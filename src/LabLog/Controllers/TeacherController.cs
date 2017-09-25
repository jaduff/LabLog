using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabLog.Models;

namespace LabLog.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserModel user)
        {
            return RedirectToAction("RoomList", "Teacher");
            //return View();
        }

        public IActionResult RoomList()
        {
            return View();
        }

        public IActionResult Room(string id)
        {
            ViewData["Message"] = "Room Page.";

            using (var db = new RoomModelContext())
            {
                RoomModel room = new RoomModel();
                room.Name = id;
                room.createComputers(20);

                foreach (ComputerModel computer in room.Computers) {
                    db.Add(computer);
                }
                
                var count = db.SaveChanges();

                ViewData["RoomName"] = id;
                ViewData["Computers"] = room.Computers;
            }



            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
