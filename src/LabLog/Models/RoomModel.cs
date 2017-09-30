using System;
using System.ComponentModel.DataAnnotations;
using LabLog.Models;
using System.Collections.Generic;
using LabLog.Domain.Events;
namespace LabLog
{
    public class RoomModel
    {
        [Required]
        [Display(Name = "Room Identifier")]
        public string Name {get; set;}
        public Guid Id {get; set;}
        public int Version {get; set;}
        public List<ComputerModel> Computers {get; set;}

        public void ApplyRoomCreatedEvent(ILabEvent e)
        {
            RoomCreatedEvent roomCreatedEvent= e.GetEventBody<RoomCreatedEvent>();
            Name = roomCreatedEvent.Name;
            Id = e.RoomId;
            Version = e.Version;
        }
        public void Replay(ILabEvent labEvent)
        {
            switch (labEvent.EventType)
            {
                case RoomCreatedEvent.EventTypeString:
                    ApplyRoomCreatedEvent(labEvent);
                break;
            }
        }


        /* some way of representing room layout? Image blob? URL to image? something fancy like a floor plan using drag and drop? */
        
    }
}