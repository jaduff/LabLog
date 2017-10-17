namespace LabLog.Domain.Entities
{
    public class Computer
    {
        public int ComputerId { get; }
        public string ComputerName { get; }
        public string RoomName { get; }

        public Computer(int computerId,
            string computerName)
        {
            ComputerId = computerId;
            ComputerName = computerName;
        }
    }
}