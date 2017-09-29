using System.Threading.Tasks;
using CheetahTesting;
using LabLog.Tests.Steps;
using Xunit;

namespace LabLog.Tests.EntityTests
{
    public class RoomTests
    {
        [Fact]
        public async Task RoomCanBeCreated()
        {
            await CTest<RoomContext>
                .Given(c => { })
                .When(i => i.CreateARoom("Test Room Name"))
                .Then(t => t.RoomIsCreated())
                .And(t => t.IdIsSet())
                .And(t => t.RoomCreatedEventRaised("Test Room Name"))
                .And(t => t.EventHasRoomId())
                .ExecuteAsync();
        }

        [Fact]
        public async Task RoomCanBeNamed()
        {
            await CTest<RoomContext>
                .Given(a => a.Room())
                .When(i => i.NameARoom())
                .Then(t => t.RoomNameChangedEventRaised())
                .And(t => t.EventHasRoomName())
                .ExecuteAsync();
        }
    }
}
