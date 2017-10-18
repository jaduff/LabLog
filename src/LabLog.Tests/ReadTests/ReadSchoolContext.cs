using System.Collections.Generic;
using LabLog.Domain.Events;
using LabLog.Domain.Entities;
using System;

namespace LabLog.ReadTests
{
    public class ReadSchoolContext
    {
        public SchoolModel School { get; set; }
        public IEnumerable<LabEvent> RetrievedEvents { get; set; }

        public string Name { get; set; }

        public Guid Id {get; set;}
    }
}