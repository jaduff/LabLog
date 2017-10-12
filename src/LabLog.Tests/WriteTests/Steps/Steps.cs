using System;
using System.Linq;
using CheetahTesting;
using LabLog.Domain.Entities;
using LabLog.Domain.Events;
using Xunit;

namespace LabLog.WriteTests.Steps
{
    public static class Steps
    {
        public static void School(this IGiven<WriteSchoolContext> given)
        {
            given.Context.School = Domain.Entities.School.Create("Test name", GetEventHandler(given.Context));
        }

        public static Action<ILabEvent> GetEventHandler(WriteSchoolContext context)
        {
            return e => context.ReceivedEvents.Add(e);
        }

        public static void AddAComputer(this IWhen<WriteSchoolContext> when, 
            int computerId, 
            string computerName)
        {
            when.Context.School.AddComputer(new Computer(computerId, computerName));
        }

        public static void AddARoom(this IWhen<WriteSchoolContext> when,
            string roomName)
        {
            when.Context.School.AddRoom(new Room(roomName));
        }

        public static void CreateASchool(this IWhen<WriteSchoolContext> when, string name)
        {
            when.Context.School = Domain.Entities.School.Create(name, GetEventHandler(when.Context));
        }

        public static void SchoolIsCreated(this IThen<WriteSchoolContext> then)
        {
            Assert.NotNull(then.Context.School);
        }

        public static void IdIsSet(this IThen<WriteSchoolContext> then)
        {
            Assert.NotNull(then.Context.School.Id);
            Assert.NotEqual(default(Guid), then.Context.School.Id);
        }

        public static void NameASchool(this IWhen<WriteSchoolContext> when)
        {
            when.Context.School.Name = "TD";
        }

        public static void VersionIs(this IThen<WriteSchoolContext> then, int version)
        {
            Assert.Equal(version, then.Context.School.Version);
        }
    }
}