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
            await CTest<ReadRoomContext>
                .Given(a => a.RoomCreatedEvent())
                .When(i => i.CreateTheRoom())
                .Then(t => t.RoomIsCreated())
                .And(t => t.IdIsSet())
                .ExecuteAsync();
        }

    }
}
