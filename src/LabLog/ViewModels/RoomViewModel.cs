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
        public string ControllerString { get; set; }

        public RoomViewModel(string controllerString, SchoolModel school, RoomModel room)
        {
            ControllerString = controllerString;
            School = school;
            Room = room;
        }
    }
}
