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
        public IActionResult Index(string id)

        {
            using (var db = new EventModelContext())
            {
                    //ViewData["Rooms"] = db.Rooms.ToList();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RoomModel room)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(room.Name + " added to database");
                //return RedirectToAction("IndexSuccess", new { message = msg});
            }
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
