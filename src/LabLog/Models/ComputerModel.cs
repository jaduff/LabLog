using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LabLog.Models
{
    public class ComputerModel
    {
        public string SerialNumber {get; set;}
        public int Position {get; set;}
        public string Name { get; set; }
        public List<ComputerUserModel> UserList { get; set; } = new List<ComputerUserModel>();
        public List<DamageModel> DamageList {get; set; } = new List<DamageModel>();

        public List<DamageModel> GetUnresolvedDamage()
        {
            List<DamageModel> unresolved = new List<DamageModel>();
            foreach (DamageModel damage in DamageList)
            {
                if (damage.Resolved == false) { unresolved.Add(damage); }
            }
            return unresolved;
        }
    }
}