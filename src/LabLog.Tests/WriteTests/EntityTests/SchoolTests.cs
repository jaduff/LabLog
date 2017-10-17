using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;

namespace LabLog.WriteTests.EntityTests
{
    public class SchoolTests
    {
        [Fact]
        public async Task SchoolCanBeCreated()
        {
            await CTest<WriteSchoolContext>
                .Given(c => { })
                .When(i => i.CreateASchool("Test School Name"))
                .Then(t => t.SchoolIsCreated())
                .And(t => t.IdIsSet())
                .And(t => t.SchoolCreatedEventRaised("Test School Name"))
                .And(t => t.EventHasSchoolId())
                .ExecuteAsync();
        }

        [Fact]
        public async Task SchoolCanBeNamed()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .When(i => i.NameASchool())
                .Then(t => t.SchoolNameChangedEventRaised())
                .And(t => t.EventHasSchoolName())
                .ExecuteAsync();
        }
    }
}
