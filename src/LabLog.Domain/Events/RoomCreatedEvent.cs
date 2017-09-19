using System;

namespace LabLog.Domain.Events
{
    public class RoomCreatedEvent : LabEvent
    {
        public RoomCreatedEvent(Guid roomId)
        {
            RoomId = roomId;
        }
        public override string EventType => "RoomCreated";
    }
}