using System;
using System.Collections.Generic;
namespace LabLog.Domain.Entities
{
    public class Room
    {
        public string RoomName { get; }
        public Guid RoomId {get;}
        public List<Computer> Computers { get; } = new List<Computer>();

        public Room(Guid roomId, string roomName)
        {
            RoomName = roomName;
            RoomId = roomId;

        }
    }
}