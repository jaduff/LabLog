using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabLog.Models;

namespace LabLog.ViewModels
{
    public class AddComputerViewModel
    {
        public SchoolModel School { get; set; }
        public RoomModel Room { get; set; }
        public ComputerModel Computer { get; set; }
    }
}
