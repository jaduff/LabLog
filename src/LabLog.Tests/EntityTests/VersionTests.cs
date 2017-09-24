using System.Threading.Tasks;
using CheetahTesting;
using LabLog.Tests.Steps;
using Xunit;

namespace LabLog.Tests.EntityTests
{
    public class VersionTests
    {
        [Fact]
        public async Task VersionStartsAt1()
        {
            await CTest<RoomContext>
                .Given(a => a.Room())
                .When(c => {})
                .Then(t => t.VersionIs(1))
                .ExecuteAsync();
        }
        
        [Fact]
        public async Task VersionIncrementsOnce()
        {
            await CTest<RoomContext>
                .Given(a => a.Room())
                .When(i => i.AddAComputer(1, "Computer One"))
                .Then(t => t.VersionIs(2))
                .ExecuteAsync();
        }

        [Fact]
        public async Task VersionIncrementsTwice()
        {
            await CTest<RoomContext>
                .Given(a => a.Room())
                .When(i => i.AddAComputer(1, "Computer One"))
                .And(i => i.NameARoom())
                .Then(t => t.VersionIs(3))
                .ExecuteAsync();
        }
    }
}