using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;

namespace LabLog.WriteTests.EntityTests
{
    public class ComputerTests
    {
        [Fact]
        public async Task SchoolCanAddComputer()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .When(i => i.AddAComputer(6, "Computer Six"))
                .Then(t => t.ComputerAddedEventRaised(6, "Computer Six"))
                .ExecuteAsync();
        }
    }
}
