using System;
using System.ComponentModel.DataAnnotations;
using LabLog.Models;
using System.Collections.Generic;
using LabLog.Domain.Events;
namespace LabLog
{
    public class SchoolModel
    {
        [Required]
        [Display(Name = "School Identifier")]
        public string Name { get; set; }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public List<RoomModel> Rooms { get; set; } = new List<RoomModel>();

        public void ApplySchoolCreatedEvent(ILabEvent e)
        {
            SchoolCreatedEvent schoolCreatedEvent = e.GetEventBody<SchoolCreatedEvent>();
            Name = schoolCreatedEvent.Name;
            Id = e.SchoolId;
            Version = e.Version;
        }

        public void ApplyRoomAddedEvent(ILabEvent e)
        {
            RoomModel room = new RoomModel();
            var body = e.GetEventBody<RoomAddedEvent>();
            room.Name = body.RoomName;
            Rooms.Add(room);
        }

        public void ReplaySchoolEvents(IEnumerable<LabEvent> eList)
        {
            foreach (LabEvent e in eList)
            {
                switch (e.EventType)
                {
                    case SchoolCreatedEvent.EventTypeString:
                        ApplySchoolCreatedEvent(e);
                        break;
                    case RoomAddedEvent.EventTypeString:
                        ApplyRoomAddedEvent(e);
                        break;
                }
            }
        }
    }
}