﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabLog.Models;

namespace LabLog.ViewModels
{
    public class SchoolListViewModel
    {
        public List<SchoolModel> Schools { get; set; }

        public SchoolListViewModel(List<SchoolModel> schools)
        {
            Schools = schools;
        }
    }
}
