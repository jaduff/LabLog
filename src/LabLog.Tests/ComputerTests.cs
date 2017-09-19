using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheetahTesting;
using ClassLogger.Domain;
using Xunit;

namespace ClassLogger.Tests
{
    public class RoomTests
    {
        [Fact]
        public async Task RoomCanAddComputer()
        {
            await CTest<RoomContext>
                .Given(a => a.Room())
                .When(i => i.AddAComputer(6))
                .Then(t => t.ComputerAddedEventRaised(6))
                .ExecuteAsync();
        }
    }

    public class RoomContext
    {
        public Room Room { get; set; }
        public List<ILabEvent> Events { get; } = new List<ILabEvent>();
    }

    public static class Steps
    {
        public static void Room(this IGiven<RoomContext> given)
        {
            given.Context.Room = new Room(@event =>
            {
                given.Context.Events.Add(@event);
            });
        }

        public static void AddAComputer(this IWhen<RoomContext> when, int computerId)
        {
            when.Context.Room.AddComputer(new Computer(computerId));
        }

        public static void ComputerAddedEventRaised(this IThen<RoomContext> then, int computerId)
        {
            Assert.Equal(1, then.Context.Events.Count());
            Assert.Equal("ComputerAdded", then.Context.Events.First().EventType);
            Assert.Equal(typeof(ComputerAddedEvent), then.Context.Events.First().GetType());
        }
    }
}
