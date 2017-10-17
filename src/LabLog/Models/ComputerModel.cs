using System;
using Microsoft.EntityFrameworkCore;
namespace LabLog.Models
{
    public class ComputerModel
    {
        public string SerialNumber {get; set;}
        public int ComputerNumber {get; set;}
        public string Name { get; set; }
    }
}