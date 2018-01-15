using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabLog.Models;

namespace LabLog.ViewModels
{
    public class ComputerViewModel
    {
        public ComputerModel computer { get; set; }
        public DamageModel newDamage { get; set; }
    }
}
