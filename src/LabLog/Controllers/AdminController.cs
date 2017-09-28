using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabLog.Models;
using LabLog.Domain.Entities;


namespace LabLog.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            ViewData["Rooms"]="";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Room formroom)
        {
            //if (ModelState.IsValid)
            //{
                // Use this so attempt to add to database only occurs when model is valid.
            //}
            int count;
            var room = LabLog.Domain.Entities.Room.Create(e => {
                using (var db = new EventModelContext())
                {
                    db.Add(e);
                    count = db.SaveChanges();
                }
            });
            ViewData["Rooms"]="";
            return View();
        }

        public IActionResult AddRoom()
        {
            return View();
        }
        public IActionResult Room()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult EventDemo()
        {
            var room = LabLog.Domain.Entities.Room.Create(e => {
                // This is an Action that stores events in the database.
                // e is the event. Store it.
            });

            return Ok();
        }
    }
}
