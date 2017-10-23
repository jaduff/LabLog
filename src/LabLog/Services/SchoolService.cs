using System;
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
        private string _controllerString;

        public SchoolService(EventModelContext db, string user, string controllerString)
        {
            _db = db;
            _user = user;
            _school = new SchoolEventHandler(_user, db);
            _controllerString = controllerString;
        }

        public async Task<List<SchoolModel>> RebuildReadModelAsync()
        {
            await DeleteReadModelFromDatabaseAsync();
            List<LabEvent> schoolCreateEvents = await _db.LabEvents.Where(o => (o.EventType == SchoolCreatedEvent.EventTypeString)).ToListAsync();

            foreach (ILabEvent e in schoolCreateEvents)
            {
                SchoolModel school = await GetSchoolAsync(e.SchoolId);
                await _db.AddAsync(school);
                await _db.SaveChangesAsync();
            }
            List<SchoolModel> schools = _db.Schools.ToList();
            return schools;
        }

        private async Task DeleteReadModelFromDatabaseAsync()
        {
            await _db.Database.ExecuteSqlCommandAsync("DELETE FROM ComputerModel;");
            await _db.Database.ExecuteSqlCommandAsync("DELETE FROM RoomModel;");
            await _db.Database.ExecuteSqlCommandAsync("DELETE FROM Schools;");
        }

        public void CreateSchool(SchoolModel school)
        {
                Domain.Entities.School.Create(school.Name, e =>
                {
                    SchoolModel _school = new SchoolModel();
                    e.EventAuthor = _user;
                    _db.Add(e);
                    _school.ApplySchoolCreatedEvent(e);
                    _db.Add(_school);
                    _db.SaveChanges();
                });
        }

        public async Task<SchoolModel> GetSchoolAsync (Guid schoolId)
        { 
            SchoolModel school = SchoolModel.GetSchoolFromDb(_db, schoolId); //TODO Make this async
            await _db.Entry(school).Collection(c => c.Rooms).LoadAsync();
            if (school.Rooms == null) { school.Rooms = new List<RoomModel>(); }
            return (school);
        }

        public async Task<SchoolViewModel> SchoolViewModelAsync(Guid schoolId)
        {
            SchoolModel school = await GetSchoolAsync(schoolId);
            SchoolViewModel schoolViewModel = new SchoolViewModel(_controllerString, school);
            return schoolViewModel;
        }

        public async Task GetSchoolRoomsAsync(SchoolModel school)
        {
            await _db.Entry(school).Collection(c => c.Rooms).LoadAsync();
        }

        public async Task<RoomModel> GetRoomAsync(SchoolModel school, string roomName)
        {
            await GetSchoolRoomsAsync(school);
            return (school.GetRoom(roomName));
        }

        public async Task GetRoomComputersAsync(RoomModel room)
        {
            await _db.Entry(room).Collection(c => c.Computers).LoadAsync();
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

            RoomViewModel roomViewModel = new RoomViewModel(_controllerString, school, room);
            return roomViewModel;
        }


    }
}
