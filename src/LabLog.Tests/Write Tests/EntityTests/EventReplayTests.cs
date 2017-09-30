using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;

namespace LabLog.WriteTests.EntityTests
{
    public class EventReplayTests
    {
        [Fact]
        public async Task NewRoomReplaysComputerAdded()
        {
            await CTest<WriteRoomContext>
                .Given(a => a.Room())
                .And(a => a.ComputerAddedEvent(6, "Computer Six"))
                .When(i => i.ReplayEvents())
                .Then(c =>
                {
                    Assert.Equal(1, c.Context.Room.Computers.Count);
                    Assert.Equal(6, c.Context.Room.Computers[0].ComputerId);
                    Assert.Equal("Computer Six", c.Context.Room.Computers[0].ComputerName);
                })
                .ExecuteAsync();
        }
    }
}