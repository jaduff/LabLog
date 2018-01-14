using System;
using System.Collections.Generic;

namespace LabLog.Domain.Events
{
    public class DamageAddedEvent : IEventBody
    {
        public DamageAddedEvent(string serialNumber, int damageId, string damageDescription)
        {
            SerialNumber = serialNumber;
            DamageDescription = DamageDescription;
            DamageId = damageId;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public DamageAddedEvent()
        {

        }
        public string EventType => EventTypeString;
        public string DamageDescription {get; set;}
        public int DamageId {get; set;}
        public string SerialNumber {get; set;}
        public const string EventTypeString ="DamageAdded";
        public DateTime TimeAssigned {get;} = DateTime.Now;
    }
}