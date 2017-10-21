using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;
using System;
using LabLog.Domain.Exceptions;

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

        [Fact]
        public async Task RoomNamesCantBeDuplicates()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .When(i => i.AddARoom("Test Room"))
                .And(i => i.Context.Delayed = () => i.AddARoom("Test Room"))
                .Then(t => 
                {
                    try
                    {
                       t.Context.Delayed();
                       Assert.False(true);
                    }
                    catch (LabException ex)
                    {
                        Assert.Throws<UniqueRoomNameException>(() => ex.NextException());
                    }
                })
                .ExecuteAsync();
            
        }
    }
}
