using System;
using System.ComponentModel.DataAnnotations;
using LabLog.Models;
using System.Collections.Generic;
using LabLog.Domain.Events;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using LabLog.Services;
using System.Threading.Tasks;

namespace LabLog
{
    public class SchoolModel
    {
        [Required]
        [Display(Name = "School Identifier")]
        public string Name { get; set; }
        public Guid Id { get; set; }
        public List<RoomModel> Rooms { get; set; } = new List<RoomModel>();

        [Obsolete("Only made public for database access. Do not use externally")]
        public int _latestVersion {get; set;}

        public void ApplySchoolCreatedEvent(ILabEvent e)
        {
            SchoolCreatedEvent schoolCreatedEvent = e.GetEventBody<SchoolCreatedEvent>();
            Name = schoolCreatedEvent.Name;
            Id = e.SchoolId;
            _latestVersion = e.Version;
        }

        public void ApplyRoomAddedEvent(ILabEvent e)
        {
            RoomModel room = new RoomModel();
            var body = e.GetEventBody<RoomAddedEvent>();
            room.Name = body.RoomName;
            room.Id = body.RoomId;
            Rooms.Add(room);
            _latestVersion = e.Version;
        }

        public void ApplyComputerAddedEvent(ILabEvent e)
        {
            ComputerModel computer = new ComputerModel();
            var body = e.GetEventBody<ComputerAddedEvent>();
            computer.Name = body.ComputerName;
            computer.SerialNumber = body.SerialNumber;
            computer.Position = body.Position;
            RoomModel room = Rooms.Find(f => (f.Id == body.RoomId));
            if (room == null) { throw new Exception("Error: Could not find a match for room with id: " + body.RoomId);}
            room.Computers.Add(computer);
            _latestVersion = e.Version;
        }

        public void ApplyStudentAssignedEvent(ILabEvent e)
        {
            var body = e.GetEventBody<StudentAssignedEvent>();
            ComputerUserModel user = new ComputerUserModel(body.TimeAssigned, body.Username); 
            RoomModel room = Rooms.Find(f => (f.Computers.Find(g => g.SerialNumber == body.SerialNumber).SerialNumber == body.SerialNumber));
            room.Computers.Find(c => (c.SerialNumber == body.SerialNumber)).UserList.Add(user);
            _latestVersion = e.Version;
        }

        public void ReplaySchoolEvent(ILabEvent e)
        {
            switch (e.EventType)
            {
                case SchoolCreatedEvent.EventTypeString:
                    ApplySchoolCreatedEvent(e);
                    break;
                case RoomAddedEvent.EventTypeString:
                    ApplyRoomAddedEvent(e);
                    break;
                case ComputerAddedEvent.EventTypeString:
                    ApplyComputerAddedEvent(e);
                    break;
                case StudentAssignedEvent.EventTypeString:
                    ApplyStudentAssignedEvent(e);
                    break;
            }

        }

        public void Update(IEnumerable<LabEvent> eventEnum)
        {
            List<LabEvent> eventList = eventEnum.ToList();
            foreach (LabEvent @event in eventList.FindAll(f => (f.Version > _latestVersion)))
            {
                ReplaySchoolEvent(@event);
            }
        }

        public RoomModel GetRoom(string roomName)
        {
            return (Rooms.Where(w => w.Name == roomName)).SingleOrDefault();
        }

        public static async Task<SchoolModel> GetSchoolFromEventsAsync(EventModelContext db, Guid schoolId)
        {
            SchoolModel school = new SchoolModel();
            var schoolEvents = await GetSchoolEventsAsync(db, schoolId);
            school.Update(schoolEvents);
            return school;
        }

        public static async Task<SchoolModel> GetSchoolFromDbAsync(EventModelContext db, Guid schoolId)
        {
            return await (db.Schools.Where(w => w.Id == schoolId)).SingleOrDefaultAsync();
        }

        public static async Task<IEnumerable<LabEvent>> GetSchoolEventsAsync(EventModelContext db, Guid schoolId)
        {
            IEnumerable<LabEvent> eventList = await db.LabEvents
                .Where(w => (w.SchoolId == schoolId))
                     .OrderBy(o => (o.Version)).ToListAsync();
            return eventList;
        }
    }
}