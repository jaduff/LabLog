using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;

namespace LabLog.WriteTests.EntityTests
{
    public class EventReplayTests
    {
        [Fact]
        public async Task NewSchoolReplaysComputerAdded()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .And(a => a.ComputerAddedEvent(6, "Computer Six"))
                .When(i => i.ReplayEvents())
                .Then(c =>
                {
                    Assert.Equal(1, c.Context.School.Computers.Count);
                    Assert.Equal(6, c.Context.School.Computers[0].ComputerId);
                    Assert.Equal("Computer Six", c.Context.School.Computers[0].ComputerName);
                })
                .ExecuteAsync();
        }

        [Fact]
        public async Task NewSchoolReplaysRoomAdded()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent("TD"))
                .When(i => i.ReplayEvents())
                .Then(c =>
                {
                    Assert.Equal(1, c.Context.School.Rooms.Count);
                    Assert.Equal("TD", c.Context.School.Rooms[0].RoomName);
                })
                .ExecuteAsync();
        }
    }
}