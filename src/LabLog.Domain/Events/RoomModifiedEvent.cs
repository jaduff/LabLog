using System;

namespace LabLog.Domain.Events
{
    public class RoomModifiedEvent : IEventBody
    {

        public RoomModifiedEvent(string roomName)
        {
            RoomName = roomName;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public RoomModifiedEvent()
        {
            
        }

        public string EventType => "RoomModified";

        public string RoomName{get;set;}
    }
}