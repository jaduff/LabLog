using System;
using System.ComponentModel.DataAnnotations;
using LabLog.Models;
using System.Collections.Generic;
using LabLog.Domain.Events;
namespace LabLog
{
    public class SchoolModel
    {
        [Required]
        [Display(Name = "School Identifier")]
        public string Name {get; set;}
        public Guid Id {get; set;}
        public int Version {get; set;}
        public List<ComputerModel> Computers {get; set;}

        public void ApplySchoolCreatedEvent(ILabEvent e)
        {
            SchoolCreatedEvent schoolCreatedEvent= e.GetEventBody<SchoolCreatedEvent>();
            Name = schoolCreatedEvent.Name;
            Id = e.SchoolId;
            Version = e.Version;
        }
        public void Replay(ILabEvent labEvent)
        {
            switch (labEvent.EventType)
            {
                case SchoolCreatedEvent.EventTypeString:
                    ApplySchoolCreatedEvent(labEvent);
                break;
            }
        }


        /* some way of representing school layout? Image blob? URL to image? something fancy like a floor plan using drag and drop? */
        
    }
}