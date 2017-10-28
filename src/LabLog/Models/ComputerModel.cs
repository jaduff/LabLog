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
    }
}