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
            given.Context.Id = new Guid("11111111-1111-1111-1111-111111111111");
            given.Context.Name = "Test School";
            SchoolCreatedEvent schoolCreatedEvent = new SchoolCreatedEvent();
            schoolCreatedEvent.Name = given.Context.Name;
            LabEvent labEvent = LabEvent.Create(given.Context.Id, 1, schoolCreatedEvent); 
            given.Context.RetrievedEvents.Add(labEvent);
        }

        public static void CreateTheSchool(this IWhen<ReadSchoolContext> when)
        {
            ILabEvent labEvent = when.Context.RetrievedEvents.First();
            when.Context.School = new SchoolModel();
            when.Context.School.ApplySchoolCreatedEvent(labEvent);
        }

        public static void School(this IGiven<ReadSchoolContext> given)
        {
            given.Context.School = new SchoolModel();
            given.Context.School.Id = new Guid("11111111-1111-1111-1111-111111111111");
            given.Context.School.Name = "Test School";
        }

        public static Action<ILabEvent> GetEventHandler(ReadSchoolContext context)
        {
            return e => context.RetrievedEvents.Append(e);
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

        public static void RoomAddedEvent(this IGiven<ReadSchoolContext> when,
            Guid roomId, string roomName)
        {
            var @event = LabEvent.Create(
                Guid.NewGuid(),
                1,
                new RoomAddedEvent(roomId, roomName));
            when.Context.RetrievedEvents.Add(@event);
        }
        public static void ComputerAddedEvent(this IGiven<ReadSchoolContext> when,
            Guid roomId, string serialNumber, string computerName, int position)
        {
            var @event = LabEvent.Create(
                Guid.NewGuid(),
                1,
                new ComputerAddedEvent(roomId, serialNumber, computerName, position));
            when.Context.RetrievedEvents.Add(@event);
        }

        public static void StudentAssignedEvent(this IGiven<ReadSchoolContext> given,
            string serialNumber, string userName)
        {
            var @event = LabEvent.Create(
                given.Context.School.Id,
                1,
                new StudentAssignedEvent(serialNumber, userName));
            given.Context.RetrievedEvents.Add(@event);
        }

        public static void DamageRecordedEvent(this IGiven<ReadSchoolContext> given,
            string roomName, string serialNumber, int damageId, string damageDescription)
        {
            var @event = LabEvent.Create(
                given.Context.School.Id,
                1,
                new DamageAddedEvent(roomName, serialNumber, damageId, damageDescription));
            given.Context.RetrievedEvents.Add(@event);
        }

        public static void ReplayEvents (this IWhen<ReadSchoolContext> when)
        {
            when.Context.School.Update(when.Context.RetrievedEvents);
        }

    }
}