using System;

namespace LabLog.Domain.Events
{
    public class SchoolNameChangedEvent : IEventBody
    {

        public SchoolNameChangedEvent(string schoolName)
        {
            SchoolName = schoolName;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public SchoolNameChangedEvent()
        {
            
        }

        public string EventType => "SchoolNameChanged";

        public string SchoolName{get;set;}
    }
}