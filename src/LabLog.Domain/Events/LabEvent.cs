using System;
using Newtonsoft.Json;

namespace LabLog.Domain.Events
{
    public class LabEvent : ILabEvent
    {
        public LabEvent()
        {

        }

        protected LabEvent(Guid schoolId, 
            int version,
            string eventType)
        {
            SchoolId = schoolId;
            Version = version;
            Timestamp = DateTimeOffset.UtcNow;
            EventType = eventType;
        }

        public static LabEvent Create<T>(Guid schoolId, 
            int version, 
            T labEvent) where T : IEventBody
            {
                var e = new LabEvent(schoolId, version, labEvent.EventType);
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

        public Guid SchoolId { get; set; }
        public int Version{get;set;}
        public DateTimeOffset Timestamp { get; set; }
        public string EventType { get; set;}
        public string EventBody { get;set;}
        public string EventAuthor { get; set; }
    }
}