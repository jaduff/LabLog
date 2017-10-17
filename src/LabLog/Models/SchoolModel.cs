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
        public List<RoomModel> Rooms { get; set; } = new List<RoomModel>();

        public void ApplySchoolCreatedEvent(ILabEvent e)
        {
            SchoolCreatedEvent schoolCreatedEvent = e.GetEventBody<SchoolCreatedEvent>();
            Name = schoolCreatedEvent.Name;
            Id = e.SchoolId;
        }

        public void ApplyRoomAddedEvent(ILabEvent e)
        {
            RoomModel room = new RoomModel();
            var body = e.GetEventBody<RoomAddedEvent>();
            room.Name = body.RoomName;
            Rooms.Add(room);
        }

        public void ApplyComputerAddedEvent(ILabEvent e)
        {
            ComputerModel computer = new ComputerModel();
            var body = e.GetEventBody<ComputerAddedEvent>();
            computer.Name = body.ComputerName;
            //something wrong with the properties of ComputerModel.
            // RoomModel room = Rooms.Find(f => (f.Id == e.)) -- No way to link computer to room? ComputerModel needs a room reference?
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

        public void ReplaySchoolEvents(IEnumerable<LabEvent> eList)
        {
            foreach (LabEvent e in eList)
            {
                ReplaySchoolEvent(e);
            }
        }
    }
}