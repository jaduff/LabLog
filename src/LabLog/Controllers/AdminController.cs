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
            var events = from _events in _db.LabEvents
                         where _events.EventType.Equals(SchoolCreatedEvent.EventTypeString)
                         select _events;
            foreach (LabEvent schoolCreatedEvent in events)
            {
                SchoolModel school = new SchoolModel();
                school.ApplySchoolCreatedEvent(schoolCreatedEvent);
                schoolList.Add(school);
            }
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
            int count;
                try
                {
                    Domain.Entities.School.Create(school.Name, e =>
                    {
                        e.EventAuthor = _user;
                        _db.Add(e);
                        count = _db.SaveChanges();
                    });
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
            SchoolModel school = new SchoolModel();
            var events = _db.LabEvents
                                    .Where(w => (w.SchoolId == id))
                                    .OrderBy(o => (o.Version));
            foreach (LabEvent schoolEvent in events)
            {
                school.Replay(schoolEvent);
            }
            
            //need to return error where no school is returned

            return View(school);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
