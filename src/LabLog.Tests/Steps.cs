using System.Linq;
using CheetahTesting;
using LabLog.Domain.Entities;
using LabLog.Domain.Events;
using Xunit;

namespace LabLog.Tests
{
    public static class Steps
    {
        public static void Room(this IGiven<RoomContext> given)
        {
            given.Context.Room = new Room(@event =>
            {
                given.Context.ReceivedEvents.Add(@event);
            });
        }

        public static void ComputerAddedEvent(this IGiven<RoomContext> given)
        {
            given.Context.PendingEvents.Add(new ComputerAddedEvent());
        }

        public static void AddAComputer(this IWhen<RoomContext> when, int computerId)
        {
            when.Context.Room.AddComputer(new Computer(computerId));
        }

        public static void ReplayEvents(this IWhen<RoomContext> when)
        {
            foreach (var pending in when.Context.PendingEvents)
            {
                when.Context.Room.Replay(pending);
            }
        }

        public static void ComputerAddedEventRaised(this IThen<RoomContext> then, int computerId)
        {
            Assert.Equal(1, then.Context.ReceivedEvents.Count());
            Assert.Equal("ComputerAdded", then.Context.ReceivedEvents.First().EventType);
            Assert.Equal(typeof(ComputerAddedEvent), then.Context.ReceivedEvents.First().GetType());
        }
    }
}