using System.Collections.Generic;
using LabLog.Domain.Events;
using LabLog.Domain.Entities;

namespace LabLog.ReadTests
{
    public class ReadRoomContext
    {
        public RoomModel Room { get; set; }
        public List<ILabEvent> RetrievedEvents { get; } = new List<ILabEvent>();
    }
}