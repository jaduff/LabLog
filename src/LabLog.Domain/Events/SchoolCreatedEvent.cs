using System;

namespace LabLog.Domain.Events
{
    public class SchoolCreatedEvent : IEventBody
    {
        public const string EventTypeString = "SchoolCreated";
        public string EventType => EventTypeString;
        public string Name { get; set; }
    }
}