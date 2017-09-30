using System.Collections.Generic;
using LabLog.Domain.Events;
using LabLog.Domain.Entities;
using System;

namespace LabLog.ReadTests
{
    public class ReadRoomContext
    {
        public RoomModel Room { get; set; }
        public List<ILabEvent> RetrievedEvents { get; } = new List<ILabEvent>();

        public string Name { get; set; }

        public Guid Id {get; set;}
        public int Version {get; set;}
    }
}