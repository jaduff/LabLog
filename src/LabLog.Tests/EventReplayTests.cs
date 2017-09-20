using System.Threading.Tasks;
using CheetahTesting;
using Xunit;

namespace LabLog.Tests
{
    public class EventReplayTests
    {
        [Fact]
        public async Task NewRoomReplaysComputerAdded()
        {
            await CTest<RoomContext>
                .Given(a => a.Room())
                .And(a => a.ComputerAddedEvent())
                .When(i => i.ReplayEvents())
                .Then(c =>
                {
                    Assert.Equal(1, c.Context.Room.Computers.Count);
                })
                .ExecuteAsync();
        }
    }
}