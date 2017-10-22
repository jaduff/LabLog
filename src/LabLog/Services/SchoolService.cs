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

        public List<SchoolModel> RebuildReadModel()
        {
            DeleteReadModelFromDatabase();
            var schoolCreateEvents = _db.LabEvents.Where(o => (o.EventType == SchoolCreatedEvent.EventTypeString));

            foreach (ILabEvent e in schoolCreateEvents)
            {
                SchoolModel school = GetSchool(e.SchoolId);
                _db.Add(school);
                _db.SaveChanges();
            }
            List<SchoolModel> schools = _db.Schools.ToList();
            return schools;
        }

        private void DeleteReadModelFromDatabase()
        {
            _db.Database.ExecuteSqlCommand("DELETE FROM ComputerModel;");
            _db.Database.ExecuteSqlCommand("DELETE FROM RoomModel;");
            _db.Database.ExecuteSqlCommand("DELETE FROM Schools;");
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

        public SchoolModel GetSchool (Guid schoolId)
        {
            SchoolModel school = SchoolModel.GetSchoolFromDb(_db, schoolId);
            _db.Entry(school).Collection(c => c.Rooms).Load();
            if (school.Rooms == null) { school.Rooms = new List<RoomModel>(); }
            return (school);
        }

        public SchoolViewModel SchoolViewModel(Guid schoolId)
        {
            SchoolModel school = GetSchool(schoolId);
            SchoolViewModel schoolViewModel = new SchoolViewModel(_controllerString, school);
            return schoolViewModel;
        }

        public void GetSchoolRooms(SchoolModel school)
        {
            _db.Entry(school).Collection(c => c.Rooms).Load();
        }

        public RoomModel GetRoom(SchoolModel school, string roomName)
        {
            GetSchoolRooms(school);
            return (school.GetRoom(roomName));
        }

        public void GetRoomComputers(RoomModel room)
        {
            _db.Entry(room).Collection(c => c.Computers).Load();
        }

        public void AddRoom(Guid schoolId, string roomName)
        {
            _school.School(GetSchool(schoolId)).AddRoom(roomName);
        }

        public void AddComputer(Guid schoolId, string roomName, ComputerModel computer)
        {
            SchoolModel school = GetSchool(schoolId);
            GetSchoolRooms(school);
            RoomModel roomModel = GetRoom(school, roomName);
            LabLog.Domain.Entities.Computer dComputer = new LabLog.Domain.Entities.Computer(computer.SerialNumber, computer.Name, computer.Position);
            _school.School(school).AddComputer(roomModel.Id, dComputer);
        }

        public List<SchoolModel> GetSchools()
        {
            return (_db.Schools.ToList());
        }

        public AddComputerViewModel AddComputerViewModel (Guid schoolId, string roomName)
        {
            AddComputerViewModel addComputerViewModel = new AddComputerViewModel();
            addComputerViewModel.School = GetSchool(schoolId);
            GetSchoolRooms(addComputerViewModel.School);
            addComputerViewModel.Room = GetRoom(addComputerViewModel.School, roomName);
            addComputerViewModel.Computer = new ComputerModel();
            return addComputerViewModel;
        }


        public RoomViewModel RoomViewModel (Guid schoolId, string roomName)
        {
            SchoolModel school = GetSchool(schoolId);
            RoomModel room = GetRoom(school, roomName);
            GetRoomComputers(room);

            RoomViewModel roomViewModel = new RoomViewModel(_controllerString, school, room);
            return roomViewModel;
        }


    }
}
