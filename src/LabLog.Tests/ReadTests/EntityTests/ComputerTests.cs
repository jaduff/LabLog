using System.Threading.Tasks;
using CheetahTesting;
using Xunit;
using LabLog.Domain.Events;
using LabLog.ReadTests.Steps;
using System;

namespace LabLog.ReadTests.EntityTests
{
    class ComputerTests
    {
        [Fact]
        public async Task ComputerCanBeAddedFromEvent()
        {
            await CTest<ReadSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent(new Guid("11111111-1111-1111-1111-111111111113"), "Test Room"))
                .And(a => a.ComputerAddedEvent(new Guid("11111111-1111-1111-1111-11111111"), "Serial", "Test Computer", 9))
                .When(i => i.ReplayEvents())
                .Then(t => {
                    Assert.Equal("Test Computer", t.Context.School.Rooms.Find(f => (f.Id == new Guid("11111111-1111-1111-1111-111111111111"))).Computers[0].Name);
                    Assert.Equal("Test Computer1", t.Context.School.Rooms.Find(f => (f.Id == new Guid("11111111-1111-1111-1111-111111111111"))).Computers[0].Name);
                })
                .ExecuteAsync();
        }

    }
}
