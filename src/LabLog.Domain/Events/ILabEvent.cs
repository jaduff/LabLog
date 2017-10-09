using System;
using System.Collections.Generic;

namespace LabLog.Domain.Events
{
    public interface ILabEvent
    {
        Guid RoomId { get; set; }
        DateTimeOffset Timestamp { get; set; }
        string EventType { get; set;}
        string EventBody{get;set;}
        int Version { get; set; }
        T GetEventBody<T>();
        void SetEventBody<T>(T eventBody);
        string EventAuthor { get; set; }
    }
}