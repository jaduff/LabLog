using System;
using Newtonsoft.Json;

namespace LabLog.Domain.Events
{
    public class LabEvent<T> : ILabEvent where T : IEventBody
    {
        public LabEvent(Guid roomId, T eventBody)
        {
            RoomId = roomId;
            Timestamp = DateTimeOffset.UtcNow;
            EventType = eventBody.EventType;
            EventBodyObject = eventBody;
        }
        public Guid RoomId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string EventType { get; set;}
        public T EventBodyObject 
        {
            get
            {
                return JsonConvert.DeserializeObject<T>(EventBody);
            }
            set
            {
                EventBody = JsonConvert.SerializeObject(value);
            }
        }
        public string EventBody { get;set;}
    }
}