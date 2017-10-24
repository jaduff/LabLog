using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabLog.Models;

namespace LabLog.ViewModels
{
    public class RoomViewModel
    {
        public SchoolModel School { get; set; }
        public RoomModel Room { get; set; }

        public RoomViewModel(SchoolModel school, RoomModel room)
        {
            School = school;
            Room = room;
        }
    }
}
