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
                .When(i => i.AddARoom("Test Room"))
                .And(i => i.AddAComputer(i.Context.School.Rooms[0].RoomId, "serial", "Computer Six", 6))
                .Then(t => t.ComputerAddedEventRaised(t.Context.School.Rooms[0].RoomId, "serial","Computer Six", 6))
                .ExecuteAsync();
        }
    }
}
