using System.Threading.Tasks;
using CheetahTesting;
using Xunit;

namespace LabLog.Tests
{
    public class ComputerTests
    {
        [Fact]
        public async Task RoomCanAddComputer()
        {
            await CTest<RoomContext>
                .Given(a => a.Room())
                .When(i => i.AddAComputer(6))
                .Then(t => t.ComputerAddedEventRaised(6))
                .ExecuteAsync();
        }
    }
}
