using System;
using System.Collections.Generic;

namespace LabLog.Domain.Events
{
    public class StudentAssignedEvent : IEventBody
    {
        public StudentAssignedEvent(string serialNumber, string username)
        {
            SerialNumber = serialNumber;
            Username = username;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public StudentAssignedEvent()
        {

        }
        public string EventType => EventTypeString;
        public string Username {get;set;}
        public string SerialNumber {get; set;}
        public const string EventTypeString ="StudentAssignedToComputer";
        public DateTime TimeAssigned {get;} = DateTime.Now;
    }
}