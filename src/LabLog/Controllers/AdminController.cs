using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabLog.Models;
using Microsoft.AspNetCore.Http;
using LabLog.Domain.Events;
using LabLog.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using LabLog.ViewModels.Admin;
using LabLog.Services;



namespace LabLog.Controllers
{
    public class AdminController : Controller
    {

        private readonly EventModelContext _db;
        private string _user = "user";
        private SchoolEventHandler _school;
        private List<SchoolModel> _schools;
        public AdminController(EventModelContext db)
        {
            _db = db;
            _school = new SchoolEventHandler(_user, _db); 
            _schools = _db.Schools.ToList();
        }

        [Route("/")]
        [Route("Admin")]
        public IActionResult Index()
        {
            return View(_schools);
        }

        [Route("/RebuildReadModel")]
        public IActionResult RebuildReadModel()
        {
            _db.Database.ExecuteSqlCommand("DELETE FROM ComputerModel;");
            _db.Database.ExecuteSqlCommand("DELETE FROM RoomModel;");
            _db.Database.ExecuteSqlCommand("DELETE FROM Schools;");
            _schools = new List<SchoolModel>();
            var eSchools = _db.LabEvents.Where(o => (o.EventType == SchoolCreatedEvent.EventTypeString));

            foreach (ILabEvent e in eSchools)
            {
                SchoolModel _school = new SchoolModel();
                var schoolEvents = _db.LabEvents
                    .Where(w => (w.SchoolId == e.SchoolId))
                    .OrderBy(o => (o.Version));
                _school.Update(schoolEvents);
                _db.Add(_school);
                _db.SaveChanges();
            }
            _schools = _db.Schools.ToList();

            return RedirectToAction("Index");
        }

        [Route("Admin/AddSchool")]
        [HttpGet]
        public IActionResult AddSchool()
        {
            return View();
        }

        [Route("Admin/AddSchool")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSchool(SchoolModel school)
        {
            try
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
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(school);
            }
            _schools = _db.Schools.ToList();

            return RedirectToAction("Index");
            
        }

        [Route("Admin/{id}/{name}/Room/{roomName}")]
        public IActionResult Room(Guid id, string roomName)
        {
            SchoolModel school = _schools.Where(s => (s.Id == id)).SingleOrDefault();
            _db.Entry(school).Collection(c => c.Rooms).Load();
            RoomModel room = school.Rooms.Where(w => (w.Name == roomName)).SingleOrDefault();
            _db.Entry(room).Collection(c => c.Computers).Load();
            
            RoomViewModel roomViewModel = new RoomViewModel(school, room);

            return View(roomViewModel);
        }

        [Route("Admin/{id}/{name?}/AddRoom")]
        [HttpGet]
        public IActionResult AddRoom()
        {
            return View();
        }

        [Route("Admin/{id}/{name?}/AddRoom")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoom(Guid id, RoomModel room)
        {
            SchoolModel schoolModel = _schools.Where(w => (w.Id == id)).SingleOrDefault();
            try
            {
                _school.School(schoolModel).AddRoom(room.Name);


            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(room);
            }

            return RedirectToAction("School", id );

        }


        [Route("Admin/{schoolID}/{name}/Room/{roomname}/AddComputer")]
        [HttpGet]
        public IActionResult AddComputer(Guid schoolID, string roomName)
        {
            AddComputerViewModel computerViewModel = new AddComputerViewModel();
            computerViewModel.School = _schools.Where(w => (w.Id == schoolID)).SingleOrDefault();
            _db.Entry(computerViewModel.School).Collection(c => c.Rooms).Load();
            computerViewModel.Room = computerViewModel.School.Rooms.Where(w => (w.Name == roomName)).SingleOrDefault();
            computerViewModel.Computer = new ComputerModel();
            return View(computerViewModel);
        }

        [HttpPost]
        [Route("Admin/{schoolID}/{name}/Room/{roomName}/AddComputer")]
        public IActionResult AddComputer(Guid schoolId, string roomName, AddComputerViewModel computerView)
        {
            SchoolModel schoolModel = _schools.Where(s => (s.Id == schoolId)).SingleOrDefault();
            _db.Entry(schoolModel).Collection(c => c.Rooms).Load();
            RoomModel room = schoolModel.Rooms.Where(w => (w.Name == roomName)).SingleOrDefault();

            try
            {
                LabLog.Domain.Entities.School school = _school.School(schoolModel);
                Domain.Entities.Computer computer = new Domain.Entities.Computer(computerView.Computer.SerialNumber, computerView.Computer.Name, computerView.Computer.Position);
                school.AddComputer(room.Id, computer);
            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(computerView);
            }


            string schoolName = schoolModel.Name;
            return RedirectToAction("Room", "Admin", new { id = schoolId, name = schoolName, roomName = roomName});
        }

        [Route("Admin/{id}/{name?}")]
        public IActionResult School(Guid id, string name)
        {
            SchoolModel school = _schools.Where(w => (w.Id == id)).SingleOrDefault();
            _db.Entry(school).Collection(c => c.Rooms).Load();
            if (school.Rooms == null)
            {
                school.Rooms = new List<RoomModel>();
            }
            return View(school);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
