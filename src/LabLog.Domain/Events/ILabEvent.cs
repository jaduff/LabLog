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
<<<<<<< HEAD
        string EventAuthor { get; set; }
=======
        string EventAuthor { get; set; } //The user who initiated the event.
>>>>>>> 3159a125319643698b3cb44543167ca0c378971d
    }
}