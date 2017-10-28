using System;
using System.Linq;
using CheetahTesting;
using LabLog.Domain.Events;
using Xunit;
using Newtonsoft.Json;

namespace LabLog.WriteTests.Steps
{
    public static class EventSteps
    {
        public static void ComputerAddedEvent(this IGiven<WriteSchoolContext> given,
            Guid roomId, string serialNumber, string computerName, int position)
        {
            given.Context.PendingEvents.Add(LabEvent.Create(Guid.NewGuid(),
                1,
                new ComputerAddedEvent(roomId, serialNumber, computerName, position)));
        }

        public static void RoomAddedEvent(this IGiven<WriteSchoolContext> given,
            Guid roomId,
            string roomName)
        {
            given.Context.PendingEvents.Add(LabEvent.Create(given.Context.School.Id,
                1,
                new RoomAddedEvent(roomId, roomName)));
        }

        public static void ReplayEvents(this IWhen<WriteSchoolContext> when)
        {
            foreach (var pending in when.Context.PendingEvents)
            {
                when.Context.School.Replay(pending);
            }
        }

        public static void ComputerAddedEventRaised(this IThen<WriteSchoolContext> then, 
            Guid roomId,
            string serialNumber,
            string computerName,
            int position)
        {
            Assert.Equal(3, then.Context.ReceivedEvents.Count);
            var @event = then.Context.ReceivedEvents[2];
            Assert.Equal(LabLog.Domain.Events.ComputerAddedEvent.EventTypeString, @event.EventType);
            ComputerAddedEvent body = @event.GetEventBody<ComputerAddedEvent>();
            Assert.Equal(position, body.Position);
            Assert.Equal(computerName, body.ComputerName);
            Assert.Equal(serialNumber, body.SerialNumber);
        }

        public static void TimeAssignedGreaterThanZero(this IThen<WriteSchoolContext> then)
        {
            StudentAssignedEvent body = then.Context.ReceivedEvents[2].GetEventBody<StudentAssignedEvent>();
            DateTime now = DateTime.Now;
            Assert.Equal(body.TimeAssigned.Date, now.Date);
        }

        public static void RoomAddedEventRaised(this IThen<WriteSchoolContext> then,
            string roomName)
        {
            Assert.Equal(2, then.Context.ReceivedEvents.Count);
            var @event = then.Context.ReceivedEvents[1];
            Assert.Equal(LabLog.Domain.Events.RoomAddedEvent.EventTypeString, @event.EventType);
            RoomAddedEvent body = @event.GetEventBody<RoomAddedEvent>();
            Assert.Equal(roomName, body.RoomName);
        }

        public static void StudentAssignedEventRaised(this IThen<WriteSchoolContext> then,
            string username)
        {
            var @event = then.Context.ReceivedEvents[3];
            Assert.Equal(LabLog.Domain.Events.StudentAssignedEvent.EventTypeString, @event.EventType);
            Assert.Equal("username", @event.GetEventBody<StudentAssignedEvent>().Username);
        }

        public static void SchoolCreatedEventRaised(this IThen<WriteSchoolContext> then, string name)
        {
            Assert.Equal(1, then.Context.ReceivedEvents.Count);
            Assert.Equal(LabLog.Domain.Events.SchoolCreatedEvent.EventTypeString, then.Context.ReceivedEvents.First().EventType);
            var body = then.Context.ReceivedEvents.First().GetEventBody<SchoolCreatedEvent>();
            Assert.NotNull(body);
            Assert.Equal(name, body.Name);
        }

        public static void EventHasSchoolId(this IThen<WriteSchoolContext> then)
        {
            Assert.Equal(then.Context.School.Id, then.Context.ReceivedEvents[0].SchoolId);
        }

        public static void SchoolNameChangedEventRaised(this IThen<WriteSchoolContext> then)
        {
            Assert.Equal(2, then.Context.ReceivedEvents.Count);
            Assert.Equal("SchoolNameChanged", then.Context.ReceivedEvents[1].EventType);
            Assert.NotNull(then.Context.ReceivedEvents.First().GetEventBody<SchoolCreatedEvent>());
        }

        public static void EventHasVersion(this IThen<WriteSchoolContext> then, 
            int eventIndex, 
            int version)
        {
            Assert.Equal(version, then.Context.ReceivedEvents[eventIndex].Version);
        }

        public static void EventHasSchoolName(this IThen<WriteSchoolContext> then)
        {
            Assert.Equal(then.Context.School.Name, JsonConvert.DeserializeObject<SchoolNameChangedEvent>(then.Context.ReceivedEvents[1].EventBody).SchoolName);
        }
    }
}