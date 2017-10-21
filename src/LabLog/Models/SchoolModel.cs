using System;
using System.ComponentModel.DataAnnotations;
using LabLog.Models;
using System.Collections.Generic;
using LabLog.Domain.Events;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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

    }
}