using System;

namespace LabLog.Domain.Events
{
    public interface ILabEvent
    {
        Guid EventId { get; set; }
        DateTimeOffset Timestamp { get; set; }
        string EventType { get; }
    }
}