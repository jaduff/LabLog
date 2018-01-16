using System.Threading.Tasks;
using CheetahTesting;
using Xunit;
using LabLog.Domain.Events;
using LabLog.ReadTests.Steps;
using System;

namespace LabLog.ReadTests.EntityTests
{
    public class ComputerTests
    {
        [Fact]
        public async Task ComputerCanBeAddedFromEvent()
        {
            await CTest<ReadSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent(new Guid("11111111-1111-1111-1111-111111111113"), "Test Room"))
                .And(a => a.ComputerAddedEvent(new Guid("11111111-1111-1111-1111-111111111113"), "Serial", "Test Computer", 9))
                .When(i => i.ReplayEvents())
                .Then(t => {
                    Assert.Equal("Test Computer", t.Context.School.Rooms.Find(f => (f.Id == new Guid("11111111-1111-1111-1111-111111111113"))).Computers[0].Name);
                    Assert.Equal("Serial", t.Context.School.Rooms.Find(f => (f.Id == new Guid("11111111-1111-1111-1111-111111111113"))).Computers[0].SerialNumber);
                    Assert.Equal(9, t.Context.School.Rooms.Find(f => (f.Id == new Guid("11111111-1111-1111-1111-111111111113"))).Computers[0].Position);

                })
                .ExecuteAsync();
        }

        [Fact]
        public async Task StudentCanBeAssignedToComputer()
        {
            await CTest<ReadSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent(new Guid("11111111-1111-1111-1111-111111111113"), "Test Room"))
                .And(a => a.ComputerAddedEvent(new Guid("11111111-1111-1111-1111-111111111113"), "Serial", "Test Computer", 9))
                .And(a => a.StudentAssignedEvent("Serial", "john.smith"))
                .When(i => i.ReplayEvents())
                .Then(t => {
                    Assert.Equal(1, t.Context.School.Rooms[0].Computers[0].UserList.Count);
                    Assert.Equal("john.smith", t.Context.School.Rooms[0].Computers[0].UserList[0].UsernameAssigned);
                })
                .ExecuteAsync();
        }

        [Fact]
        public async Task DamageCanBeRecordedOnComputer()
        {
            await CTest<ReadSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent(new Guid("11111111-1111-1111-1111-111111111113"), "Test Room"))
                .And(a => a.ComputerAddedEvent(new Guid("11111111-1111-1111-1111-111111111113"), "Serial", "Test Computer", 9))
                .And(a => a.DamageRecordedEvent("roomName", "Serial", 2, "computer damaged"))
                .When(i => i.ReplayEvents())
                .Then(t => {
                    Assert.Equal(1, t.Context.School.Rooms[0].Computers[0].DamageList.Count);
                    Assert.Equal("computer damaged", t.Context.School.Rooms[0].Computers[0].DamageList[0].Description);
                    Assert.Equal(2, t.Context.School.Rooms[0].Computers[0].DamageList[0].DamageID);
                })
                .ExecuteAsync();
        }


    }
}
