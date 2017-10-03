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
        public static void Room(this IGiven<WriteRoomContext> given)
        {
            given.Context.Room = Domain.Entities.Room.Create("Test name", "User", GetEventHandler(given.Context));
        }

        public static Action<ILabEvent> GetEventHandler(WriteRoomContext context)
        {
            return e => context.ReceivedEvents.Add(e);
        }

        public static void AddAComputer(this IWhen<WriteRoomContext> when, 
            int computerId, 
            string computerName)
        {
            when.Context.Room.AddComputer(new Computer(computerId, computerName), "User");
        }        

        public static void CreateARoom(this IWhen<WriteRoomContext> when, string name)
        {
            when.Context.Room = Domain.Entities.Room.Create(name, "User", GetEventHandler(when.Context));
        }

        public static void RoomIsCreated(this IThen<WriteRoomContext> then)
        {
            Assert.NotNull(then.Context.Room);
        }

        public static void IdIsSet(this IThen<WriteRoomContext> then)
        {
            Assert.NotNull(then.Context.Room.Id);
            Assert.NotEqual(default(Guid), then.Context.Room.Id);
        }

        public static void NameARoom(this IWhen<WriteRoomContext> when)
        {
            when.Context.Room.SetName("TD", "User");
        }

        public static void VersionIs(this IThen<WriteRoomContext> then, int version)
        {
            Assert.Equal(version, then.Context.Room.Version);
        }
    }
}