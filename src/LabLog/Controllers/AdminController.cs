﻿using System;
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
        public AdminController(EventModelContext db)
        {
            _db = db;
            _school = new SchoolEventHandler(_user, _db); 
        }

        [Route("/")]
        [Route("Admin")]
        public IActionResult Index()
        {
            List<SchoolModel> schoolList = new List<SchoolModel>();
            schoolList = _db.Schools.ToList();

            return View(schoolList);
        }

        [Route("/RebuildReadModel")]
        public IActionResult RebuildReadModel()
        {
            _db.Database.ExecuteSqlCommand("DELETE FROM ComputerModel;");
            _db.Database.ExecuteSqlCommand("DELETE FROM RoomModel;");
            _db.Database.ExecuteSqlCommand("DELETE FROM Schools;");
            var eSchools = _db.LabEvents.Where(o => (o.EventType == SchoolCreatedEvent.EventTypeString));

            foreach (ILabEvent e in eSchools)
            {
                SchoolModel _school = new SchoolModel();
                var schoolEvents = _db.LabEvents
                    .Where(w => (w.SchoolId == e.SchoolId))
                    .OrderBy(o => (o.Version));
                _school.ReplaySchoolEvents(schoolEvents);
                _db.Add(_school);
                _db.SaveChanges();
            }

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

            return RedirectToAction("Index");
            
        }

        [Route("Admin/{id}/{name}/Room/{roomName}")]
        public IActionResult Room(Guid id, string roomName)
        {
            SchoolModel school = _db.Schools.Include(i => i.Rooms).Where(s => (s.Id == id)).SingleOrDefault();
            RoomModel room = school.Rooms.Where(w => (w.Name == roomName)).SingleOrDefault();
            _db.Entry(room).Collection(c => c.Computers).Load();
            
            RoomViewModel roomViewModel = new RoomViewModel(school, room);
            //need to return error where no school is returned

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
            try
            {
                _school.School(id).AddRoom(room.Name);


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
            computerViewModel.School = _db.Schools.Include(i => i.Rooms).Where(w => (w.Id == schoolID)).SingleOrDefault();
            computerViewModel.Room = computerViewModel.School.Rooms.Where(w => (w.Name == roomName)).SingleOrDefault();
            computerViewModel.Computer = new ComputerModel();
            return View(computerViewModel);
        }

        [HttpPost]
        [Route("Admin/{schoolID}/{name}/Room/{RoomName}/AddComputer")]
        public IActionResult AddComputer(Guid schoolId, string roomName, AddComputerViewModel computerView)
        {
            SchoolModel schoolModel = computerView.School;
            RoomModel room = computerView.Room;

            try
            {
                LabLog.Domain.Entities.School school = _school.School(schoolId);
                Domain.Entities.Computer computer = new Domain.Entities.Computer(computerView.Computer.SerialNumber, computerView.Computer.Name, computerView.Computer.Position);
                school.AddComputer(room.Id, computer);
            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(computerView);
            }


            string schoolName = computerView.School.Name;
            return RedirectToAction("Room", "Admin", new { id = schoolId, name = schoolName, roomName = roomName});
        }

        [Route("Admin/{id}/{name?}")]
        public IActionResult School(Guid id, string name)
        {
            SchoolModel school = _db.Schools.Include(i => (i.Rooms)).Where(w => (w.Id == id)).SingleOrDefault();
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
