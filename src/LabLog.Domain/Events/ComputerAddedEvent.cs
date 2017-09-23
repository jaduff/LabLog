using System;

namespace LabLog.Domain.Events
{
    public class ComputerAddedEvent : IEventBody
    {
        public ComputerAddedEvent(string computerName)
        {
            ComputerName = computerName;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public ComputerAddedEvent()
        {
            
        }
        public string EventType => "ComputerAdded";
        public string ComputerName{get;set;}
    }
}