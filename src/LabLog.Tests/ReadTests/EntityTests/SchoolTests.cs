using System.Threading.Tasks;
using CheetahTesting;
using Xunit;
using LabLog.Domain.Events;
using LabLog.ReadTests.Steps;
using System;

namespace LabLog.ReadTests.EntityTests
{
    class SchoolTests
    {
        [Fact]
        public async Task SchoolCanBeCreatedFromEvent()
        {
            await CTest<ReadSchoolContext>
                .Given(a => a.SchoolCreatedEvent())
                .When(i => i.CreateTheSchool())
                .Then(t => t.SchoolIsCreated())
                .And(t => t.IdIsSet())
                .ExecuteAsync();
        }

    }
}
