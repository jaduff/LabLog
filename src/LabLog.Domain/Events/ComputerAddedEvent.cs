using System;
using System.Collections.Generic;

namespace LabLog.Domain.Events
{
    public class ComputerAddedEvent : IEventBody
    {
        public ComputerAddedEvent(int computerId, 
            string computerName)
        {
            ComputerName = computerName;
            ComputerId = computerId;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public ComputerAddedEvent()
        {
            
        }
        public string EventType => "ComputerAdded";
        public string ComputerName{get;set;}
        public int ComputerId { get; set; }
    }
}