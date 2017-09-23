using System;

namespace LabLog.Domain.Events
{
    public class RoomNameChangedEvent : IEventBody
    {

        public RoomNameChangedEvent(string roomName)
        {
            RoomName = roomName;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public RoomNameChangedEvent()
        {
            
        }

        public string EventType => "RoomNameChanged";

        public string RoomName{get;set;}
    }
}