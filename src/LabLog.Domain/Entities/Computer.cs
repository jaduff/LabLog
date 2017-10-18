using System;
namespace LabLog.Domain.Entities
{
    public class Computer
    {
        public string ComputerName { get; }
        public Guid RoomId {get;}
        public string SerialNumber {get;}
        public int Position {get;}

        public Computer(Guid roomId, string serialNumber,
            string computerName, int position)
        {
            RoomId = roomId;
            SerialNumber = serialNumber;
            ComputerName = computerName;
            Position = position;
        }
    }
}