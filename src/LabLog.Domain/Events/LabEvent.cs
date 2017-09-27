using System;
using Newtonsoft.Json;

namespace LabLog.Domain.Events
{
    public class LabEvent : ILabEvent
    {
        protected LabEvent(Guid roomId, 
            int version,
            string eventType)
        {
            RoomId = roomId;
            Version = version;
            Timestamp = DateTimeOffset.UtcNow;
            EventType = eventType;
        }

        public static LabEvent Create<T>(Guid roomId, 
            int version, 
            T labEvent) where T : IEventBody
            {
                var e = new LabEvent(roomId, version, labEvent.EventType);
                e.SetEventBody(labEvent);
                return e;
            }

        public T GetEventBody<T>()
        {
            return JsonConvert.DeserializeObject<T>(EventBody);
        }

        public void SetEventBody<T>(T eventBody)
        {
            EventBody = JsonConvert.SerializeObject(eventBody);
        }

        public Guid RoomId { get; set; }
        public int Version{get;set;}
        public DateTimeOffset Timestamp { get; set; }
        public string EventType { get; set;}
        public string EventBody { get;set;}
    }
}