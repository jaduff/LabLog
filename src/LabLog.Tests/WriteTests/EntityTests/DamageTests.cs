using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;
using LabLog.Domain.Exceptions;
using System;

namespace LabLog.WriteTests.EntityTests
{
    public class DamageTests
    {
        [Fact]
        public async Task DamageCanBeRecorded()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomWithComputer())
                .When(i => i.RecordDamage(i.Context.School.Rooms[0].RoomName, i.Context.School.Rooms[0].Computers[0].SerialNumber, "Computer damaged"))
                .Then(t => t.DamageAddedEventRaised("Computer damaged"))
                .And(t => t.ComputerLastDamageIdIs(1))
                .ExecuteAsync();
        }

        [Fact]
        public async Task AdditionalDamageGetsNewID()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomWithComputer())
                .When(i => i.RecordDamage(i.Context.School.Rooms[0].RoomName, i.Context.School.Rooms[0].Computers[0].SerialNumber, "Computer damaged"))
                .And(i => i.RecordDamage(i.Context.School.Rooms[0].RoomName, i.Context.School.Rooms[0].Computers[0].SerialNumber, "keys missing"))
                .Then(t => t.DamageAddedEventRaised("Computer damaged"))
                .And(t => t.ComputerLastDamageIdIs(2))
                .ExecuteAsync();
        }

    }
}
