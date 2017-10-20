using System.Threading.Tasks;
using CheetahTesting;
using LabLog.WriteTests.Steps;
using Xunit;
using LabLog.Domain.Exceptions;
using System;

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

        [Fact]
        public async Task SerialMustBeUnique()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .When(i => i.AddARoom("Test Room"))
                .And(i => i.AddARoom("Test Room2"))
                .And(i => i.AddAComputer(i.Context.School.Rooms[0].RoomId, "serial", "Computer Six", 6))
                .And(i => i.Context.Delayed = () => i.AddAComputer(i.Context.School.Rooms[1].RoomId, "serial", "Computer Seven", 7))
                .Then(t =>
                {
                    try
                    {
                     t.Context.Delayed();   
                     Assert.False(true);
                    }
                    catch(LabException ex)
                    {
                        Assert.Throws<UniqueComputerSerialException>(() => ex.NextException());
                    }
                })
                .ExecuteAsync();
        }

        [Fact]
        public async Task UniqueSerialExceptionAndUniqueComputerClassPositionExceptionsCanBeThrown()
        {
            await CTest<WriteSchoolContext>
                .Given(a => a.School())
                .When(i => i.AddARoom("Test Room"))
                .And(i => i.AddAComputer(i.Context.School.Rooms[0].RoomId, "serial", "Computer Six", 6))
                .And(i => i.Context.Delayed = () => i.AddAComputer(i.Context.School.Rooms[0].RoomId, "serial", "Computer Seven", 6))
                .Then(t =>
                {
                    try
                    {
                     t.Context.Delayed();   
                     Assert.False(true);
                    }
                    catch(LabException ex)
                    {
                        Assert.Throws<UniqueComputerSerialException>(() => ex.NextException());
                        Assert.Throws<UniqueComputerClassPositionException>(() => ex.NextException());
                    }
                })
                .ExecuteAsync();

        }

    }
}
