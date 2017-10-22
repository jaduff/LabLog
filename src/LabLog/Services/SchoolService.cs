using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabLog.Models;
using Microsoft.EntityFrameworkCore;
using LabLog.Domain.Events;


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
            return (SchoolModel.GetSchoolFromDb(_db, schoolId));
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

        public void AddComputer(SchoolModel school, RoomModel room, ComputerModel computer)
        {
            LabLog.Domain.Entities.Computer dComputer = new LabLog.Domain.Entities.Computer(computer.SerialNumber, computer.Name, computer.Position);
            _school.School(school).AddComputer(room.Id, dComputer);
        }

        public List<SchoolModel> GetSchools()
        {
            return (_db.Schools.ToList());
        }
    }
}
