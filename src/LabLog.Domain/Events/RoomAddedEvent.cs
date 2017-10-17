using System;
using System.Collections.Generic;

namespace LabLog.Domain.Events
{
    public class RoomAddedEvent : IEventBody
    {
        public RoomAddedEvent(string roomName)
        {
            RoomName = roomName;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public RoomAddedEvent()
        {
            
        }
        public string EventType => EventTypeString;
        public string RoomName{get;set;}
        public const string EventTypeString ="RoomAdded";
    }
}