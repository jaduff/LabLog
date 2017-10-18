using System;
namespace LabLog.Domain.Entities
{
    public class Computer
    {
        public string ComputerName { get; }
        public string SerialNumber {get;}
        public int Position {get;}

        public Computer(string serialNumber,
            string computerName, int position)
        {
            SerialNumber = serialNumber;
            ComputerName = computerName;
            Position = position;
        }
    }
}