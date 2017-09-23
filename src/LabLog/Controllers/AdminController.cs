using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabLog.Models;

namespace LabLog.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index(string id)

        {
            using (var db = new RoomModelContext())
            {
                List<RoomModel> rooms = db.Rooms;
            }
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
    }
}
