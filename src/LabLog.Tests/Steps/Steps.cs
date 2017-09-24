using System;
using System.Linq;
using CheetahTesting;
using LabLog.Domain.Entities;
using LabLog.Domain.Events;
using Xunit;
using Newtonsoft.Json;

namespace LabLog.Tests.Steps
{
    public static class Steps
    {
        public static void Room(this IGiven<RoomContext> given)
        {
            given.Context.Room = Domain.Entities.Room.Create(GetEventHandler(given.Context));
        }

        public static Action<ILabEvent> GetEventHandler(RoomContext context)
        {
            return e => context.ReceivedEvents.Add(e);
        }

        public static void AddAComputer(this IWhen<RoomContext> when, 
            int computerId, 
            string computerName)
        {
            when.Context.Room.AddComputer(new Computer(computerId, computerName));
        }        

        public static void CreateARoom(this IWhen<RoomContext> when)
        {
            when.Context.Room = Domain.Entities.Room.Create(GetEventHandler(when.Context));
        }

        public static void RoomIsCreated(this IThen<RoomContext> then)
        {
            Assert.NotNull(then.Context.Room);
        }

        public static void IdIsSet(this IThen<RoomContext> then)
        {
            Assert.NotNull(then.Context.Room.Id);
            Assert.NotEqual(default(Guid), then.Context.Room.Id);
        }

        public static void NameARoom(this IWhen<RoomContext> when)
        {
            when.Context.Room.Name = "TD";
        }
        public static void EventHasRoomName(this IThen<RoomContext> then)
        {
            Assert.Equal(then.Context.Room.Name, JsonConvert.DeserializeObject<RoomNameChangedEvent>(then.Context.ReceivedEvents[1].EventBody).RoomName);
        }

        public static void VersionIs(this IThen<RoomContext> then, int version)
        {
            Assert.Equal(version, then.Context.Room.Version);
        }
    }
}