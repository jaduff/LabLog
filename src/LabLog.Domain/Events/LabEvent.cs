using System;

namespace LabLog.Domain.Events
{
    public abstract class LabEvent : ILabEvent
    {
        protected LabEvent()
        {
            EventId = Guid.NewGuid();
            Timestamp = DateTimeOffset.UtcNow;
        }
        public Guid EventId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public abstract string EventType { get; }
    }
}