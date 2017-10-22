using LabLog.Domain.Events;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LabLog.Domain.Exceptions;

namespace LabLog.Services

{
    public class SchoolEventHandler
    {
        private EventModelContext _db;
        private string _user;
        public SchoolEventHandler(string user, EventModelContext db)
        {
            _db = db;
            _user = user;
        }
        public Domain.Entities.School School (SchoolModel schoolModel)
        {
                Guid schoolId = schoolModel.Id;
                List<LabEvent> eventList = _db.LabEvents.Where(o => (o.SchoolId == schoolId)).ToList();
                Domain.Entities.School school = new Domain.Entities.School(eventList, e =>
                {
                    e.EventAuthor = _user;
                    _db.Add(e);

                    schoolModel.Update(eventList);
                    schoolModel.ReplaySchoolEvent(e); //Is there some way of combining these two? Need to think through use cases.
                    _db.SaveChanges();
                });
                return school;
        }

    }
}