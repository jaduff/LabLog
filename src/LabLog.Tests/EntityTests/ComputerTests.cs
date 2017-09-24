using System.Threading.Tasks;
using CheetahTesting;
using LabLog.Tests.Steps;
using Xunit;

namespace LabLog.Tests.EntityTests
{
    public class ComputerTests
    {
        [Fact]
        public async Task RoomCanAddComputer()
        {
            await CTest<RoomContext>
                .Given(a => a.Room())
                .When(i => i.AddAComputer(6, "Computer Six"))
                .Then(t => t.ComputerAddedEventRaised(6, "Computer Six"))
                .ExecuteAsync();
        }
    }
}
