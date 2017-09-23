using System.Threading.Tasks;
using CheetahTesting;
using Xunit;

namespace LabLog.Tests
{
    public class RoomTests
    {
        [Fact]
        public async Task RoomCanBeCreated()
        {
            await CTest<RoomContext>
                .Given(c => { })
                .When(i => i.CreateARoom())
                .Then(t => t.RoomIsCreated())
                .And(t => t.IdIsSet())
                .And(t => t.RoomCreatedEventRaised())
                .And(t => t.EventHasRoomId())
                .ExecuteAsync();
        }

        [Fact]
        public async Task RoomCanBeNamed()
        {
            await CTest<RoomContext>
                .Given(a => a.Room())
                .When(i => i.NameARoom())
                .Then(t => t.RoomModifiedEventRaised())
                .And(t => t.EventHasRoomName())
                .ExecuteAsync();
        }
    }
}
