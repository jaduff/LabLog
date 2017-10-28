using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabLog.Models;
using LabLog.Services;
using LabLog.ViewModels;

namespace LabLog.Controllers
{
    public class TeacherController : Controller
    {
        private readonly EventModelContext _db;
        private string _user = "user";
        private SchoolService _schoolService;
        public TeacherController(EventModelContext db)
        {
            _db = db;
            _schoolService = new SchoolService(db, _user);
        }

        [Route("Teacher")]
        public async Task<IActionResult> Index()
        {
            SchoolListViewModel schoolListViewModel = new SchoolListViewModel(await _schoolService.GetSchoolsAsync());
            return View(schoolListViewModel);
        }

        [Route("Teacher/{schoolId}/{name}/Room/{roomName}")]
        public async Task<IActionResult> Room(Guid schoolId, string roomName)
        {
            SchoolModel school = await _schoolService.GetSchoolAsync(schoolId);
            RoomModel room = await _schoolService.GetRoomAsync(school, roomName);
            await _schoolService.GetRoomComputersAsync(room);

            RoomViewModel roomViewModel = new RoomViewModel(school, room);

            return View(roomViewModel);
        }

        [Route("Teacher/{schoolId}/{name}/Room/{roomName}")]
        [HttpPost]
        public async Task<IActionResult> AssignStudents(Guid schoolId, string roomName) //TODO need to pull in the model
        {
            //TODO How do I get the data from the form, which is a list of
            // usernames, and the computers to assign them to.
            //How does this work with the View, and with ViewModels?
            SchoolModel school = await _schoolService.GetSchoolAsync(schoolId);
            RoomModel room = await _schoolService.GetRoomAsync(school, roomName);
            return RedirectToAction("Room", "Teacher", new { id = schoolId, name = school.Name, roomName = roomName});
        }


        [Route("Teacher/{schoolId}/{name?}")]
        public async Task<IActionResult> School(Guid schoolId, string name)
        {
            SchoolModel school = await _schoolService.GetSchoolAsync(schoolId);
            await _schoolService.GetSchoolRoomsAsync(school);
            if (school.Rooms == null)
            {
                school.Rooms = new List<RoomModel>();
            }
            SchoolViewModel schoolViewModel = new SchoolViewModel(school);
            return View(schoolViewModel);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
