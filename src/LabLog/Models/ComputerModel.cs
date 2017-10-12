using System;
using Microsoft.EntityFrameworkCore;
namespace LabLog.Models
{
    public class ComputerModel
    {
        public string SerialNumber {get; set;}
        public string School {get; set;}
        public int ComputerNumber {get; set;}

    }
}