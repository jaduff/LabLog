using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;
using System;

namespace LabLog.WriteTests.EntityTests
{
    public class EventReplayTests
    {
        [Fact]
        public async Task NewSchoolReplaysComputerAdded()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent(new Guid("11111111-1111-1111-1111-111111111111"), "Test Room2"))
                .And(a => a.ComputerAddedEvent(new Guid("11111111-1111-1111-1111-111111111112"), "serial", "Computer Six", 6))
                .When(i => i.ReplayEvents())
                .Then(c =>
                {
                    Assert.Equal(1, c.Context.School.Rooms.Count);
                    Assert.Equal("Test Room2", c.Context.School.Rooms[0].RoomName);
                    Assert.Equal(new Guid("11111111-1111-1111-1111-111111111111"), c.Context.School.Rooms[0].RoomId);
                    Assert.Equal(1, c.Context.School.Computers.Count);
                    Assert.Equal(6, c.Context.School.Computers[0].Position);
                    Assert.Equal("Computer Six", c.Context.School.Computers[0].ComputerName);
                    Assert.Equal(new Guid("11111111-1111-1111-1111-111111111112"), c.Context.School.Computers[0].RoomId);
                })
                .ExecuteAsync();
        }

        [Fact]
        public async Task NewSchoolReplaysRoomAdded()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent(new Guid("11111111-1111-1111-1111-111111111111"), "TD"))
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