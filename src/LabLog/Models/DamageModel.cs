using System;
namespace LabLog.Models
{
    public class DamageModel
    {
        public int DamageID {get; set;}
        public string ReportedBy {get; set;}
        public string Description {get; set;}
        public bool Resolved {get; set;}
        public int GLPITicketNum {get; set;}
    }

}