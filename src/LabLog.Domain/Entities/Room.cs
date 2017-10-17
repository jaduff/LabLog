using System;
namespace LabLog.Domain.Entities
{
    public class Room
    {
        public string RoomName { get; }
        public Guid RoomId {get;}

        public Room(Guid id, string roomName)
        {
            RoomName = roomName;
            RoomId = RoomId;

        }
    }
}