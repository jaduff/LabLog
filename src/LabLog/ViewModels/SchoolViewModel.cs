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

        public SchoolViewModel (SchoolModel school)
        {
            School = school;
        }
    }
}
