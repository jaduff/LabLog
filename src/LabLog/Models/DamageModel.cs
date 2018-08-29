using System;
using Microsoft.EntityFrameworkCore;
using LabLog.Models;
namespace LabLog.Models

{
    public class DamageModel
    {
        public Guid DamageId {get; set;}
        public string ReportedBy {get; set;}
        public string Description {get; set;}
        public bool Resolved {get; set;}
        public int GLPITicketNum {get; set;}
    }

}