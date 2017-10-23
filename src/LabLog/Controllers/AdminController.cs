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
using LabLog.ViewModels;
using LabLog.Services;



namespace LabLog.Controllers
{
    public class AdminController : Controller
    {

        private readonly EventModelContext _db;
        private string _user = "user";
        private SchoolService _schoolService;
        private const string _controllerString = "Admin";
        public AdminController(EventModelContext db)
        {
            _db = db;
            _schoolService = new SchoolService(db, _user, _controllerString);
        }

        [Route("/")] // This is temporary. Remove when teacher view is implemented.
        [Route("Admin")]
        public async Task<IActionResult> Index()
        {
            SchoolListViewModel schoolList = new SchoolListViewModel(_controllerString, await _schoolService.GetSchoolsAsync());  
            return View(schoolList);
        }

        [Route("/RebuildReadModel")]
        public async Task<IActionResult> RebuildReadModel()
        {
            await _schoolService.RebuildReadModelAsync();
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
        public async Task<IActionResult> Room(Guid schoolId, string roomName)
        {
            RoomViewModel roomViewModel = await _schoolService.RoomViewModelAsync(schoolId, roomName);

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
        public async Task<IActionResult> AddRoom(Guid schoolId, RoomModel room)
        {
            try
            {
                await _schoolService.AddRoomAsync(schoolId, room.Name);
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
        public async Task<IActionResult> AddComputer(Guid schoolId, string roomName)
        {
            AddComputerViewModel computerViewModel = await _schoolService.AddComputerViewModelAsync(schoolId, roomName);
            return View(computerViewModel);
        }

        [HttpPost]
        [Route("Admin/{schoolId}/{name}/Room/{roomName}/AddComputer")]
        public async Task<IActionResult> AddComputer(Guid schoolId, string roomName, AddComputerViewModel computerView)
        {

            SchoolModel school = _db.Schools.Where(w => w.Id == schoolId).SingleOrDefault();
            try
            {
                await _schoolService.AddComputerAsync(schoolId, roomName, computerView.Computer);
            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.LabMessage;
                return View(computerView);
            }

            return RedirectToAction("Room", "Admin", new { id = schoolId, name = school.Name, roomName = roomName});
        }

        [Route("Admin/{schoolId}/{name?}")]
        public async Task<IActionResult> School(Guid schoolId, string name)
        {
            SchoolViewModel schoolView = await _schoolService.SchoolViewModelAsync(schoolId);
            return View(schoolView);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
