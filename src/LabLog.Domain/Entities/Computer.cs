using System;
using System.Collections.Generic;
namespace LabLog.Domain.Entities
{
    public class Computer
    {
        public string ComputerName { get; }
        public string SerialNumber {get;}
        public int Position {get;}
        public List<Damage> DamageList { get; } = new List<Damage>();

        public Computer(string serialNumber,
            string computerName, int position)
        {
            SerialNumber = serialNumber;
            ComputerName = computerName;
            Position = position;
        }

        public int GetLastDamageId()
        {
            int damageId = 0;
            foreach (Damage damage in DamageList)
            {
                if (damage.DamageId > damageId) { damageId = damage.DamageId; }
            }
            return damageId;
        }

    }
}