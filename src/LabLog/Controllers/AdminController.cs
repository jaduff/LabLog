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
        public AdminController(EventModelContext db)
        {
            _db = db;
            _schoolService = new SchoolService(db, _user);
        }

        [Route("/")] // This is temporary. Remove when teacher view is implemented.
        [Route("Admin")]
        public async Task<IActionResult> Index()
        {
            SchoolListViewModel schoolList = new SchoolListViewModel(await _schoolService.GetSchoolsAsync());  
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
            RoomViewModel roomViewModel = await _schoolService.GetRoomViewModel(schoolId, roomName);

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

        [Route("Admin/{schoolId}/{name}/Room/{roomName}")]
        [HttpPost]
        public async Task<IActionResult> AssignStudents(Guid schoolId,
                                                        string roomName,
                                                        //[FromForm] TestModel assignStudent) //TODO need to pull in the model
                                                        List<AssignStudentViewModel> assignStudentView)
        {
            SchoolModel school = await _schoolService.GetSchoolAsync(schoolId);
            RoomModel room = await _schoolService.GetRoomAsync(school, roomName);
            foreach (var c in assignStudentView)
            {
                await _schoolService.AssignStudentToComputerAsync(schoolId, roomName, c.SerialNumber, c.Username);
            }
            return RedirectToAction("Room", "Admin", new { schoolId = schoolId, name = school.Name, roomName = roomName });
        }

        [Route("Admin/{schoolId}/{name}/{roomName}/{position}")]
        [HttpPost]
        public async Task<IActionResult> ComputerView(Guid schoolId, string name, string roomName, int position, ComputerViewModel computerView)
        {
            await _schoolService.RecordDamage(schoolId, roomName, position, computerView.newDamage);
            return RedirectToAction("ComputerView", "Admin", new { schoolId = schoolId, name = name, roomName = roomName, position = position });
        }

        [Route("Admin/{schoolId}/{name}/{roomName}/{position}")]
        public async Task<IActionResult> ComputerView(Guid schoolId, string roomName, int position)
        {
            ComputerViewModel computerView = new ComputerViewModel();
            computerView.computer = await _schoolService.GetComputerAsync(schoolId, roomName, position);
            computerView.School = await _schoolService.GetSchoolAsync(schoolId);
            computerView.Room = await _schoolService.GetRoomAsync(computerView.School, roomName);
            return View(computerView);
        }

        [Route("Admin/{schoolId}/{name}/{roomName}/{position}/{damageId}")]
        [HttpPost]
        public async Task<IActionResult> DamageView(Guid schoolId, string name, string roomName, int position, ComputerViewModel computerView, Guid damageId, DamageViewModel damageView)
        {
            ComputerModel computer = await _schoolService.GetComputerAsync(schoolId, roomName, position);
            DamageModel editDamage = await _schoolService.GetDamageAsync(computer, damageView.Damage.DamageId);
            editDamage.ReportedBy = damageView.Damage.ReportedBy;
            editDamage.Description = damageView.Damage.Description;
            editDamage.Resolved = damageView.Damage.Resolved;
            editDamage.GLPITicketNum = damageView.Damage.GLPITicketNum;
            await _schoolService.RecordDamage(schoolId, roomName, position, editDamage);
            return RedirectToAction("DamageView", "Admin", new { schoolId = schoolId, name = name, roomName = roomName, position = position, damageId = damageId });
        }

        [Route("Admin/{schoolId}/{name}/{roomName}/{position}/{damageId}")]
        public async Task<IActionResult> DamageView(Guid schoolId, string roomName, int position, Guid damageId)
        {
            DamageViewModel damageView = new DamageViewModel();
            damageView.computer = await _schoolService.GetComputerAsync(schoolId, roomName, position);
            damageView.School = await _schoolService.GetSchoolAsync(schoolId);
            damageView.Room = await _schoolService.GetRoomAsync(damageView.School, roomName);
            damageView.Damage = await _schoolService.GetDamageAsync(damageView.computer, damageId);
            return View(damageView);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
