using System;

namespace ClassLogger.Domain
{
    public abstract class LabEvent : ILabEvent
    {
        public LabEvent()
        {
            EventId = Guid.NewGuid();
            Timestamp = DateTimeOffset.UtcNow;
        }
        public Guid EventId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public abstract string EventType { get; }
    }
}