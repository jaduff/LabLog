using System;
using System.Linq;
using CheetahTesting;
using LabLog.Domain.Entities;
using LabLog.Domain.Events;
using Xunit;

namespace LabLog.ReadTests.Steps
{
    public static class Steps
    {
        public static void SchoolCreatedEvent(this IGiven<ReadSchoolContext> given)
        {
            given.Context.Id = new Guid("11111111-1111-1111-1111-11111111");
            given.Context.Name = "Test School";
            SchoolCreatedEvent schoolCreatedEvent = new SchoolCreatedEvent();
            schoolCreatedEvent.Name = given.Context.Name;
            LabEvent labEvent = LabEvent.Create(given.Context.Id, 1, schoolCreatedEvent); 
            given.Context.RetrievedEvents.Add(labEvent);
        }

        public static void CreateTheSchool(this IWhen<ReadSchoolContext> when)
        {
            ILabEvent labEvent = when.Context.RetrievedEvents[0];
            when.Context.School = new SchoolModel();
            when.Context.School.ApplySchoolCreatedEvent(labEvent);
        }

        public static void School(this IGiven<ReadSchoolContext> given)
        {
        }

        public static Action<ILabEvent> GetEventHandler(ReadSchoolContext context)
        {
            return e => context.RetrievedEvents.Add(e);
        }

        public static void SchoolIsCreated(this IThen<ReadSchoolContext> then)
        {
            Assert.NotNull(then.Context.School);
            Assert.Equal(then.Context.School.Name, then.Context.Name);
            Assert.Equal(then.Context.School.Id, then.Context.Id);
        }

        public static void IdIsSet(this IThen<ReadSchoolContext> then)
        {
            Assert.NotNull(then.Context.School.Id);
            Assert.NotEqual(default(Guid), then.Context.School.Id);
        }

    }
}