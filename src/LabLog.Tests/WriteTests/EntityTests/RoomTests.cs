using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;

namespace LabLog.WriteTests.EntityTests
{
    public class RoomTests
    {
        [Fact]
        public async Task SchoolCanAddRoom()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .When(i => i.AddARoom("TD"))
                .Then(t => t.RoomAddedEventRaised("TD"))
                .ExecuteAsync();
        }
    }
}
