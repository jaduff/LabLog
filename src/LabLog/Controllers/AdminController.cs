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
        private SchoolService _schoolService;
        public AdminController(EventModelContext db)
        {
            _db = db;
            _schoolService = new SchoolService(db, _user);
        }

        [Route("/")] //This is temporary. Remove when teacher view is implemented.
        [Route("Admin")]
        public IActionResult Index()
        {
            
            return View(_schoolService.GetSchools());
        }

        [Route("/RebuildReadModel")]
        public IActionResult RebuildReadModel()
        {
            _schoolService.RebuildReadModel();
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
                _schoolService.CreateSchool(school);
            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(school);
            }
            return RedirectToAction("Index");
            
        }

        [Route("Admin/{schoolId}/{name}/Room/{roomName}")]
        public IActionResult Room(Guid schoolId, string roomName)
        {
            SchoolModel school = _schoolService.GetSchool(schoolId);
            RoomModel room = _schoolService.GetRoom(school, roomName);
            _schoolService.GetRoomComputers(room);
            
            RoomViewModel roomViewModel = new RoomViewModel(school, room);

            return View(roomViewModel);
        }

        [Route("Admin/{schoolId}/{name?}/AddRoom")]
        [HttpGet]
        public IActionResult AddRoom()
        {
            return View();
        }

        [Route("Admin/{schoolId}/{name?}/AddRoom")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoom(Guid schoolId, RoomModel room)
        {
            try
            {
                _schoolService.AddRoom(schoolId, room.Name);
            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(room);
            }

            return RedirectToAction("School", schoolId );

        }


        [Route("Admin/{schoolId}/{name}/Room/{roomname}/AddComputer")]
        [HttpGet]
        public IActionResult AddComputer(Guid schoolId, string roomName)
        {
            AddComputerViewModel computerViewModel = new AddComputerViewModel();
            computerViewModel.School = _schoolService.GetSchool(schoolId);
            _schoolService.GetSchoolRooms(computerViewModel.School);
            computerViewModel.Room = _schoolService.GetRoom(computerViewModel.School, roomName);
            computerViewModel.Computer = new ComputerModel();
            return View(computerViewModel);
        }

        [HttpPost]
        [Route("Admin/{schoolId}/{name}/Room/{roomName}/AddComputer")]
        public IActionResult AddComputer(Guid schoolId, string roomName, AddComputerViewModel computerView)
        {
            SchoolModel school = _schoolService.GetSchool(schoolId);
            _schoolService.GetSchoolRooms(school);
            RoomModel room = _schoolService.GetRoom(school, roomName);

            try
            {
                _schoolService.AddComputer(school, room, computerView.Computer);
            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(computerView);
            }

            return RedirectToAction("Room", "Admin", new { id = schoolId, name = school.Name, roomName = roomName});
        }

        [Route("Admin/{schoolId}/{name?}")]
        public IActionResult School(Guid schoolId, string name)
        {
            SchoolModel school = _schoolService.GetSchool(schoolId);
            _schoolService.GetSchoolRooms(school);
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
