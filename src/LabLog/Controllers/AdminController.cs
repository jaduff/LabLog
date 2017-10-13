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


namespace LabLog.Controllers
{
    public class AdminController : Controller
    {

        private readonly EventModelContext _db;
        private string _user = "user";
        public AdminController(EventModelContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<SchoolModel> schoolList = new List<SchoolModel>();
            schoolList = _db.Schools.ToList();

            return View(schoolList);
        }

        [HttpGet]
        public IActionResult AddSchool()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSchool(SchoolModel school)
        {
            try
            {
                Domain.Entities.School.Create(school.Name, e =>
                {
                    e.EventAuthor = _user;
                    _db.Add(e);
                });
                _db.Add(school);
                _db.SaveChanges();
            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(school);
            }

            return RedirectToAction("Index");
            
        }

        [Route("School/{id}/{name?}")]
        public IActionResult School(Guid id, string name)
        {
            List<RoomModel> roomList = new List<RoomModel>();
            return View(roomList);
        }

        [Route("School/{id}/{name?}/Room/{roomName?}")]
        public IActionResult Room(Guid id, string name)
        {
            RoomModel room = new RoomModel();
            //need to return error where no school is returned

            return View(room);
        }

        [Route("School/{id}/{name?}/AddRoom")]
        [HttpGet]
        public IActionResult AddRoom()
        {
            return View();
        }

        [Route("School/{id}/{name?}/AddRoom")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoom(Guid id, RoomModel room)
        {
            List<LabEvent> eventList = _db.LabEvents.Where(o => (o.SchoolId == id)).ToList();
            room.Id = new Guid();

            try
            {
                Domain.Entities.School school = new Domain.Entities.School(id, eventList, e =>
                {
                    e.EventAuthor = _user;
                    _db.Add(e);
                });
                school.AddRoom(room.Name);
                
                _db.Add(room);
                _db.SaveChanges();
            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(room);
            }

            return RedirectToAction("School", id );

        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RebuildReadModel()
        {
            var schools = _db.Schools.AsEnumerable();
            foreach (SchoolModel school in schools)
            {
                _db.Remove(school);
            }
            _db.SaveChanges();
            var eSchools = _db.LabEvents.Where(o => (o.EventType == SchoolCreatedEvent.EventTypeString));

            foreach (ILabEvent e in eSchools)
            {
                SchoolModel _school = new SchoolModel();
                var schoolEvents = _db.LabEvents
                    .Where(w => (w.SchoolId == e.SchoolId))
                    .OrderBy(o => (o.Version));
                _school.ReplaySchoolEvents(schoolEvents);
                _db.Add(_school);
            }
            _db.SaveChanges();
            
            return RedirectToAction("Index");
        }

    }
}
