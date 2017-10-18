
using System.Threading.Tasks;
using CheetahTesting;
using Xunit;
using LabLog.Domain.Events;
using LabLog.ReadTests.Steps;
using System;

namespace LabLog.ReadTests.EntityTests
{
    class RoomTests 
    {
        [Fact]
        public async Task RoomCanBeCreatedFromEvent()
        {
            await CTest<ReadSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent(new Guid("11111111-1111-1111-1111-111111111113"), "Test Room"))
                .When(i => i.ReplayEvents())
                .Then(t => {
                    Assert.Equal(new Guid("11111111-1111-1111-1111-111111111113"), t.Context.School.Rooms[0].Id);
                    Assert.Equal("Test Room", t.Context.School.Rooms[0].Name);
                })
                .ExecuteAsync();
        }

    }
}
