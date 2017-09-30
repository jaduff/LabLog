using System.Threading.Tasks;
using CheetahTesting;
using Xunit;

namespace LabLog.ReadTests.rEntityTests
{
    class RoomTests
    {
        [Fact]
        public async Task RoomCanBeCreatedFromEvent()
        {
            await CTest<ReadRoomContext>
                .Given(C => { })
                .When(i => i.ReceiveRoomCreatedEvent())
                .Then(t => t.RoomIsInstantiated())
                .And(t => t.RoomHasId())
                .ExecuteAsync();
        }

    }
}
