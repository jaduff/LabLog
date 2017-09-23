using System;

namespace LabLog.Domain.Events
{
    public class RoomCreatedEvent : IEventBody
    {
        public string EventType => "RoomCreated";
    }
}