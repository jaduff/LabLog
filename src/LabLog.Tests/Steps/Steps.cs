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

        public static void ComputerAddedEvent(this IGiven<RoomContext> given,
            int computerId, string computerName)
        {
            given.Context.PendingEvents.Add(new LabEvent<ComputerAddedEvent>(Guid.NewGuid(),
                1,
                new ComputerAddedEvent(computerId, computerName)));
        }

        public static void AddAComputer(this IWhen<RoomContext> when, 
            int computerId, 
            string computerName)
        {
            when.Context.Room.AddComputer(new Computer(computerId, computerName));
        }

        public static void ReplayEvents(this IWhen<RoomContext> when)
        {
            foreach (var pending in when.Context.PendingEvents)
            {
                when.Context.Room.Replay(pending);
            }
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

        public static void ComputerAddedEventRaised(this IThen<RoomContext> then, 
            int computerId,
            string computerName)
        {
            Assert.Equal(2, then.Context.ReceivedEvents.Count);
            var @event = then.Context.ReceivedEvents[1];
            Assert.Equal("ComputerAdded", @event.EventType);
            Assert.Equal(typeof(LabEvent<ComputerAddedEvent>), @event.GetType());
            ComputerAddedEvent body = ((LabEvent<ComputerAddedEvent>)@event).EventBodyObject;
            Assert.Equal(computerId, body.ComputerId);
            Assert.Equal(computerName, body.ComputerName);
        }

        public static void RoomCreatedEventRaised(this IThen<RoomContext> then)
        {
            Assert.Equal(1, then.Context.ReceivedEvents.Count);
            Assert.Equal("RoomCreated", then.Context.ReceivedEvents.First().EventType);
            Assert.Equal(typeof(LabEvent<RoomCreatedEvent>), then.Context.ReceivedEvents.First().GetType());
        }

        public static void EventHasRoomId(this IThen<RoomContext> then)
        {
            Assert.Equal(then.Context.Room.Id, then.Context.ReceivedEvents[0].RoomId);
        }
        public static void NameARoom(this IWhen<RoomContext> when)
        {
            when.Context.Room.Name = "TD";
        }
        public static void EventHasRoomName(this IThen<RoomContext> then)
        {
            Assert.Equal(then.Context.Room.Name, JsonConvert.DeserializeObject<RoomNameChangedEvent>(then.Context.ReceivedEvents[1].EventBody).RoomName);
        }

        public static void RoomNameChangedEventRaised(this IThen<RoomContext> then)
        {
            Assert.Equal(2, then.Context.ReceivedEvents.Count);
            Assert.Equal("RoomNameChanged", then.Context.ReceivedEvents[1].EventType);
            Assert.Equal(typeof(LabEvent<RoomCreatedEvent>), then.Context.ReceivedEvents.First().GetType());
        }

        public static void VersionIs(this IThen<RoomContext> then, int version)
        {
            Assert.Equal(version, then.Context.Room.Version);
        }

        public static void EventHasVersion(this IThen<RoomContext> then, 
            int eventIndex, 
            int version)
        {
            Assert.Equal(version, then.Context.ReceivedEvents[eventIndex].Version);
        }
    }
}