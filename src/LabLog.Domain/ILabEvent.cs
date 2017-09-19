using System;

namespace ClassLogger.Domain
{
    public interface ILabEvent
    {
        Guid EventId { get; set; }
        DateTimeOffset Timestamp { get; set; }
        string EventType { get; }
    }
}