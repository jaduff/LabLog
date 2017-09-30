using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;

namespace LabLog.WriteTests.EntityTests
{
    public class RoomTests
    {
        [Fact]
        public async Task RoomCanBeCreated()
        {
            await CTest<WriteRoomContext>
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
            await CTest<WriteRoomContext>
                .Given(a => a.Room())
                .When(i => i.NameARoom())
                .Then(t => t.RoomNameChangedEventRaised())
                .And(t => t.EventHasRoomName())
                .ExecuteAsync();
        }
    }
}
