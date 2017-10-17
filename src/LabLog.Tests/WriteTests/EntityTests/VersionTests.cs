using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;

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
                .When(i => i.AddAComputer(1, "Computer One"))
                .Then(t => t.VersionIs(2))
                .And(t => t.EventHasVersion(0, 1))
                .ExecuteAsync();
        }

        [Fact]
        public async Task VersionIncrementsTwice()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .When(i => i.AddAComputer(1, "Computer One"))
                .And(i => i.NameASchool())
                .Then(t => t.VersionIs(3))
                .And(t => t.EventHasVersion(0, 1))
                .And(t => t.EventHasVersion(1, 2))
                .And(t => t.EventHasVersion(2, 3))
                .ExecuteAsync();
        }
    }
}