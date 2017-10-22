using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabLog.Models;

namespace LabLog.ViewModels
{
    public class SchoolViewModel
    {
        public SchoolModel School { get; set; }
        public string ControllerString { get; set; }

        public SchoolViewModel (string controllerString, SchoolModel school)
        {
            ControllerString = controllerString;
            School = school;
        }
    }
}
