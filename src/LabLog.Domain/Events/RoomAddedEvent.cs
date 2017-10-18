using System;
using System.Collections.Generic;

namespace LabLog.Domain.Events
{
    public class RoomAddedEvent : IEventBody
    {
        public RoomAddedEvent(Guid roomId, string roomName)
        {
            RoomId = roomId;
            RoomName = roomName;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public RoomAddedEvent()
        {
            
        }
        public string EventType => EventTypeString;
        public string RoomName{get;set;}
        public Guid RoomId {get; set;}
        public const string EventTypeString ="RoomAdded";
    }
}