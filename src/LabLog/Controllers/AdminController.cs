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
        public AdminController(EventModelContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<RoomModel> roomList = new List<RoomModel>();
            var events = from _events in _db.LabEvents
                         where _events.EventType.Equals(RoomCreatedEvent.EventTypeString)
                         select _events;
            foreach (LabEvent roomCreatedEvent in events)
            {
                RoomModel room = new RoomModel();
                room.ApplyRoomCreatedEvent(roomCreatedEvent);
                roomList.Add(room);
            }
            return View(roomList);
        }

        public IActionResult AddRoom()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoom(RoomModel room)
        {
            //if (ModelState.IsValid)
            //{
            // Use this so attempt to add to database only occurs when model is valid.
            //}
            int count;
            try
            {
                Domain.Entities.Room.Create(room.Name, e =>
                {
                    _db.Add(e);
                    count = _db.SaveChanges();
                });
            }
            catch (LabException ex)
            {
                ViewData["message"] = ex.Message;
                return View();
            }

            return RedirectToAction("Index");
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
