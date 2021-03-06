﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabLog.Models;
using Microsoft.EntityFrameworkCore;
using LabLog.Domain.Events;
using LabLog.ViewModels;


namespace LabLog.Services
{
    public class SchoolService
    {
        private EventModelContext _db;
        private string _user;
        private SchoolEventHandler _school;

        public SchoolService(EventModelContext db, string user)
        {
            _db = db;
            _user = user;
            _school = new SchoolEventHandler(_user, db);
        }

        public async Task<List<SchoolModel>> RebuildReadModelAsync()
        {
            await DeleteReadModelFromDatabaseAsync();
            List<LabEvent> schoolCreateEvents = await _db.LabEvents.Where(o => (o.EventType == SchoolCreatedEvent.EventTypeString)).ToListAsync();

            foreach (ILabEvent e in schoolCreateEvents)
            {
                SchoolModel school = await SchoolModel.GetSchoolFromEventsAsync(_db, e.SchoolId);
                await _db.AddAsync(school);
                await _db.SaveChangesAsync();
            }
            List<SchoolModel> schools = _db.Schools.ToList();
            return schools;
        }

        private async Task DeleteReadModelFromDatabaseAsync()
        {
            List<Task> _list = new List<Task>();
            _list.Add(_db.Database.ExecuteSqlCommandAsync("DELETE FROM DamageModel;"));
            _list.Add(_db.Database.ExecuteSqlCommandAsync("DELETE FROM ComputerUserModel;"));
            _list.Add(_db.Database.ExecuteSqlCommandAsync("DELETE FROM ComputerModel;"));
            _list.Add(_db.Database.ExecuteSqlCommandAsync("DELETE FROM RoomModel;"));
            _list.Add(_db.Database.ExecuteSqlCommandAsync("DELETE FROM Schools;"));
            await Task.WhenAll(_list);
        }

        public void CreateSchool(SchoolModel school)
        {
                Domain.Entities.School.Create(school.Name, e =>
                {
                    SchoolModel schoolModel = new SchoolModel();
                    e.EventAuthor = _user;
                    _db.Add(e);
                    schoolModel.ApplySchoolCreatedEvent(e);
                    _db.Add(schoolModel);
                    _db.SaveChanges();
                });
        }

        public async Task<SchoolModel> GetSchoolAsync (Guid schoolId)
        { 
            SchoolModel school = await SchoolModel.GetSchoolFromDbAsync(_db, schoolId);
            await _db.Entry(school).Collection(c => c.Rooms).LoadAsync();
            if (school.Rooms == null) { school.Rooms = new List<RoomModel>(); }
            return (school);
        }

        public async Task<SchoolViewModel> SchoolViewModelAsync(Guid schoolId)
        {
            SchoolModel school = await GetSchoolAsync(schoolId);
            SchoolViewModel schoolViewModel = new SchoolViewModel(school);
            return schoolViewModel;
        }

        public async Task GetSchoolRoomsAsync(SchoolModel school)
        {
            await _db.Entry(school).Collection(c => c.Rooms).LoadAsync();
        }

        public async Task<RoomModel> GetRoomAsync(SchoolModel school, string roomName)
        {
            await GetSchoolRoomsAsync(school);
            RoomModel room = school.GetRoom(roomName);
            await _db.Entry(room).Collection(c => c.Computers).LoadAsync();
            return (room);
        }

        public async Task<List<ComputerModel>> GetRoomComputersAsync(RoomModel room)
        {
            await _db.Entry(room).Collection(c => c.Computers).LoadAsync();
            return room.Computers.ToList();
        }

        public async Task AddRoomAsync(Guid schoolId, string roomName)
        {
            _school.School( await GetSchoolAsync(schoolId)).AddRoom(roomName);
        }

        public async Task AddComputerAsync(Guid schoolId, string roomName, ComputerModel computer)
        {
            SchoolModel school = await GetSchoolAsync(schoolId);
            await GetSchoolRoomsAsync(school);
            RoomModel roomModel = await GetRoomAsync(school, roomName);
            LabLog.Domain.Entities.Computer dComputer = new LabLog.Domain.Entities.Computer(computer.SerialNumber, computer.Name, computer.Position);
            _school.School(school).AddComputer(roomModel.Id, dComputer);
        }

        public async Task<List<SchoolModel>> GetSchoolsAsync()
        {
            return (await _db.Schools.ToListAsync());
        }

        public async Task<AddComputerViewModel> AddComputerViewModelAsync (Guid schoolId, string roomName)
        {
            AddComputerViewModel addComputerViewModel = new AddComputerViewModel();
            addComputerViewModel.School = await GetSchoolAsync(schoolId);
            await GetSchoolRoomsAsync(addComputerViewModel.School);
            addComputerViewModel.Room = await GetRoomAsync(addComputerViewModel.School, roomName);
            addComputerViewModel.Computer = new ComputerModel();
            return addComputerViewModel;
        }


        public async Task<RoomViewModel> RoomViewModelAsync (Guid schoolId, string roomName)
        {
            SchoolModel school = await GetSchoolAsync(schoolId);
            RoomModel room = await GetRoomAsync(school, roomName);
            await GetRoomComputersAsync(room);

            RoomViewModel roomViewModel = new RoomViewModel(school, room);
            return roomViewModel;
        }

        public async Task AssignStudentToComputerAsync(Guid schoolId, string roomName, string serialNumber, string username)
        {
            //TODO get data added to DB
            SchoolModel school = await GetSchoolAsync(schoolId);
            RoomModel room = await GetRoomAsync(school, roomName);
            List<ComputerModel> computers = await GetRoomComputersAsync(room);
            _school.School(school).AssignStudent(username, serialNumber);
        }

        public async Task<RoomViewModel> GetRoomViewModel(Guid schoolId, string roomName)
        {
            SchoolModel school = await GetSchoolAsync(schoolId);
            RoomModel room = await GetRoomAsync(school, roomName);
            await GetRoomComputersAsync(room);

            RoomViewModel roomViewModel = new RoomViewModel(school, room);
            foreach (ComputerModel computer in room.Computers)
            {
                roomViewModel.AssignStudentView.Add(new AssignStudentViewModel());
            }

            return roomViewModel;
        }

        public async Task<ComputerModel> GetComputerAsync(Guid schoolId, string roomName, int position)
        {
            SchoolModel school = await GetSchoolAsync(schoolId);
            RoomModel room = await GetRoomAsync(school, roomName);
            await GetRoomComputersAsync(room);
            ComputerModel computer = room.Computers.Where(w => (w.Position == position)).SingleOrDefault();
            await _db.Entry(computer).Collection(c => c.UserList).LoadAsync();
            await _db.Entry(computer).Collection(c => c.DamageList).LoadAsync();
            return computer;
        }

        public async Task RecordDamage(Guid schoolId, string roomName, int position, DamageModel damage)
        {
            SchoolModel school = await GetSchoolAsync(schoolId);
            RoomModel room = await GetRoomAsync(school, roomName);
            ComputerModel computer = await GetComputerAsync(schoolId, roomName, position);
            _school.School(school).RecordDamage(roomName, computer.SerialNumber, damage.Description);
        }

        public async Task<DamageModel> GetDamageAsync (ComputerModel computer, int damageId)
        {
            await _db.Entry(computer).Collection(c => c.DamageList).LoadAsync();
            DamageModel damage = computer.DamageList.Where(w => w.DamageId == damageId).SingleOrDefault();
            return damage;
        }

    }
}
