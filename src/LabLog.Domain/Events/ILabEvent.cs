using System;

namespace LabLog.Domain.Events
{
    public interface ILabEvent
    {
        Guid RoomId { get; set; }
        DateTimeOffset Timestamp { get; set; }
        string EventType { get; }
    }
}