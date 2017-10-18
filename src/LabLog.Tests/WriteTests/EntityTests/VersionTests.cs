using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;
using System;

namespace LabLog.WriteTests.EntityTests
{
    public class VersionTests
    {
        [Fact]
        public async Task VersionStartsAt1()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .When(c => {})
                .Then(t => t.VersionIs(1))
                .ExecuteAsync();
        }
        
        [Fact]
        public async Task VersionIncrementsOnce()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent(new Guid("11111111-1111-1111-1111-111111111116"), "test room"))
                .When(i => i.ReplayEvents())
                .And(i => i.AddAComputer(new Guid("11111111-1111-1111-1111-111111111116"), "serial", "Computer One", 1))
                .Then(t => t.VersionIs(2))
                .And(t => t.EventHasVersion(0, 1))
                .ExecuteAsync();
        }

        [Fact]
        public async Task VersionIncrementsTwice()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .And(a => a.RoomAddedEvent(new Guid("11111111-1111-1111-1111-111111111118"), "test room"))
                .When(i => i.ReplayEvents())
                .And(i => i.AddAComputer(new Guid("11111111-1111-1111-1111-111111111118"), "serial", "Computer One", 1))
                .And(i => i.NameASchool())
                .Then(t => t.VersionIs(3))
                .And(t => t.EventHasVersion(0, 1))
                .And(t => t.EventHasVersion(1, 2))
                .And(t => t.EventHasVersion(2, 3))
                .ExecuteAsync();
        }
    }
}