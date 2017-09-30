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
            string name = "test room";
            Guid guid = new Guid("11111111-1111-1111-1111-111111111111");
            int version = 1;

            await CTest<ReadRoomContext>
                .Given(a => a.RoomCreatedEvent(guid, version, name))
                .When(i => i.CreateTheRoom())
                .Then(t => t.RoomIsCreated())
                .And(t => t.IdIsSet())
                .And(t => t.RoomIdMatches(guid))
                .And(t => t.RoomNameMatches(name))
                .And(t => t.RoomVersionMatches(version))
                .ExecuteAsync();
        }

    }
}
