using System;

namespace LabLog.Domain.Events
{
    public class RoomCreatedEvent : IEventBody
    {
        public const string EventTypeString = "RoomCreated";
        public string EventType => EventTypeString;
        public string Name { get; set; }
    }
}