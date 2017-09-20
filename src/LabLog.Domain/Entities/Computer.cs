namespace LabLog.Domain.Entities
{
    public class Computer
    {
        public int ComputerId { get; }

        public Computer(int computerId)
        {
            ComputerId = computerId;
        }
    }
}