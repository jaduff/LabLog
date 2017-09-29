using System;
using System.Linq;
using CheetahTesting;
using LabLog.Domain.Events;
using Xunit;
using Newtonsoft.Json;

namespace LabLog.Tests.Steps
{
    public static class EventSteps
    {
        public static void ComputerAddedEvent(this IGiven<WriteRoomContext> given,
            int computerId, string computerName)
        {
            given.Context.PendingEvents.Add(LabEvent.Create(Guid.NewGuid(),
                1,
                new ComputerAddedEvent(computerId, computerName)));
        }

        public static void ReplayEvents(this IWhen<WriteRoomContext> when)
        {
            foreach (var pending in when.Context.PendingEvents)
            {
                when.Context.Room.Replay(pending);
            }
        }

        public static void ComputerAddedEventRaised(this IThen<WriteRoomContext> then, 
            int computerId,
            string computerName)
        {
            Assert.Equal(2, then.Context.ReceivedEvents.Count);
            var @event = then.Context.ReceivedEvents[1];
            Assert.Equal(LabLog.Domain.Events.ComputerAddedEvent.EventTypeString, @event.EventType);
            ComputerAddedEvent body = @event.GetEventBody<ComputerAddedEvent>();
            Assert.Equal(computerId, body.ComputerId);
            Assert.Equal(computerName, body.ComputerName);
        }

        public static void RoomCreatedEventRaised(this IThen<WriteRoomContext> then, string name)
        {
            Assert.Equal(1, then.Context.ReceivedEvents.Count);
            Assert.Equal(LabLog.Domain.Events.RoomCreatedEvent.EventTypeString, then.Context.ReceivedEvents.First().EventType);
            var body = then.Context.ReceivedEvents.First().GetEventBody<RoomCreatedEvent>();
            Assert.NotNull(body);
            Assert.Equal(name, body.Name);
        }

        public static void EventHasRoomId(this IThen<WriteRoomContext> then)
        {
            Assert.Equal(then.Context.Room.Id, then.Context.ReceivedEvents[0].RoomId);
        }

        public static void RoomNameChangedEventRaised(this IThen<WriteRoomContext> then)
        {
            Assert.Equal(2, then.Context.ReceivedEvents.Count);
            Assert.Equal("RoomNameChanged", then.Context.ReceivedEvents[1].EventType);
            Assert.NotNull(then.Context.ReceivedEvents.First().GetEventBody<RoomCreatedEvent>());
        }

        public static void EventHasVersion(this IThen<WriteRoomContext> then, 
            int eventIndex, 
            int version)
        {
            Assert.Equal(version, then.Context.ReceivedEvents[eventIndex].Version);
        }

        public static void EventHasRoomName(this IThen<WriteRoomContext> then)
        {
            Assert.Equal(then.Context.Room.Name, JsonConvert.DeserializeObject<RoomNameChangedEvent>(then.Context.ReceivedEvents[1].EventBody).RoomName);
        }
    }
}