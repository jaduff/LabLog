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

        public static void Room(this IGiven<WriteSchoolContext> given,
            string roomName)
        {
            given.Context.School.AddRoom(roomName);
        }

        public static void RoomWithComputer(this IGiven<WriteSchoolContext> given)
        {
            given.Context.School.AddRoom("Test room");
            Guid roomId = given.Context.School.Rooms[0].RoomId;
            Computer computer = new Computer("serialNumber", "computerName", 5);
            given.Context.School.AddComputer(roomId, computer);
        }

        public static Action<ILabEvent> GetEventHandler(WriteSchoolContext context)
        {
            return e => context.ReceivedEvents.Add(e);
        }

        public static void AddAComputer(this IWhen<WriteSchoolContext> when, 
            Guid roomId,
            string serialNumber,
            string computerName,
            int position)
        {
            when.Context.School.AddComputer(roomId, new Computer(serialNumber, computerName, position));
        }

        public static void AssignAStudent(this IWhen<WriteSchoolContext> when,
    string username, string serialNumber)
        {
            when.Context.School.AssignStudent(serialNumber, username);
        }


        public static void AddARoom(this IWhen<WriteSchoolContext> when,
            string roomName)
        {
            when.Context.School.AddRoom(roomName);
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

        public static void RecordDamage(this IWhen<WriteSchoolContext> when, string roomName, string serialNumber, string damageDescription)
        {
            when.Context.School.RecordDamage(roomName, serialNumber, damageDescription);
        }
        public static void UpdateDamageTicket(this IWhen<WriteSchoolContext> when,
            string roomName,
            string serialNumber,
            int damageId,
            string ticket)
        {
            when.Context.School.UpdateDamageTicket(roomName, serialNumber, damageId, ticket);
        }
    }
}