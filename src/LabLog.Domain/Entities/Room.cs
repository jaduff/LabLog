using System;
namespace LabLog.Domain.Entities
{
    public class Room
    {
        public string RoomName { get; }
        public Guid RoomId {get;}

        public Room(Guid roomId, string roomName)
        {
            RoomName = roomName;
            RoomId = roomId;

        }
    }
}