using System.Collections.Generic;
using LabLog.Domain.Events;
using LabLog.Domain.Entities;

namespace LabLog.Tests
{
    using Room = LabLog.Domain.Entities.Room;
    public class RoomContext
    {
        public Room Room { get; set; }
        public List<ILabEvent> ReceivedEvents { get; } = new List<ILabEvent>();
        public List<ILabEvent> PendingEvents { get; } = new List<ILabEvent>();
    }
}