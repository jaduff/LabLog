namespace LabLog.Domain.Entities
{
    public class Room
    {
        public string RoomName { get; }

        public Room(string roomName)
        {
            RoomName = roomName;
        }
    }
}