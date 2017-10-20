using LabLog.Domain.Events;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public Domain.Entities.School School (Guid schoolId)
        {
                List<LabEvent> eventList = _db.LabEvents.Where(o => (o.SchoolId == schoolId)).ToList();
                Domain.Entities.School school = new Domain.Entities.School(eventList, e =>
                {
                    e.EventAuthor = _user;
                    _db.Add(e);

                    SchoolModel schoolModel = _db.Schools.Where(w => (w.Id == schoolId)).SingleOrDefault();
                    schoolModel.ReplaySchoolEvent(e);
                    _db.SaveChanges();
                });
                return school;
        }
    }
}