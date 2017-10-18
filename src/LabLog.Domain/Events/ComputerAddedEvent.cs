using System;
using System.Collections.Generic;

namespace LabLog.Domain.Events
{
    public class ComputerAddedEvent : IEventBody
    {
        public ComputerAddedEvent(Guid roomId, string serialNumber, 
            string computerName, int position)
        {
            RoomId = roomId;
            ComputerName = computerName;
            SerialNumber = serialNumber;
            Position = position;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public ComputerAddedEvent()
        {
            
        }
        public string EventType => EventTypeString;
        public string ComputerName{get;set;}
        public string SerialNumber { get; set; }
        public const string EventTypeString ="ComputerAdded";
        public Guid RoomId {get; set;}
        public int Position {get; set;}
    }
}