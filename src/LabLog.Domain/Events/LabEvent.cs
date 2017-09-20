using System;

namespace LabLog.Domain.Events
{
    public abstract class LabEvent : ILabEvent
    {
        protected LabEvent()
        {
            RoomId = Guid.NewGuid();
            Timestamp = DateTimeOffset.UtcNow;
        }
        public Guid RoomId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public abstract string EventType { get; }
    }
}