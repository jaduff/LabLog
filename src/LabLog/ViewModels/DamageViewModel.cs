using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabLog.Models;

namespace LabLog.ViewModels
{
    public class DamageViewModel
    {
        public ComputerModel computer { get; set; }
        public SchoolModel School { get; set; }
        public RoomModel Room { get; set; }
        public DamageModel Damage { get; set; }
    }
}
