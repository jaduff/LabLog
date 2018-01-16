using System;
using System.Collections.Generic;
using System.Text;

namespace LabLog.Domain.Entities
{
    public class Damage
    {
        public int DamageId { get; }
        public string DamageDescription { get; set;  }
        public bool Resolved { get; set; } = false;

        public Damage(int damageId, string damageDescription)
        {
            DamageId = damageId;
            DamageDescription = damageDescription;
        }
    }
}
