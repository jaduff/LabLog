using System;
using System.Linq;
using CheetahTesting;
using LabLog.Domain.Entities;
using LabLog.Domain.Events;
using Xunit;

namespace LabLog.ReadTests.Steps
{
    public static class Steps
    {
        public static void RoomCreatedEvent(this IGiven<ReadRoomContext> given)
        {
            RoomCreatedEvent roomCreatedEvent = new RoomCreatedEvent();
            roomCreatedEvent.Name = "Test Room";
            LabEvent labEvent = LabEvent.Create(new Guid("11111111-1111-1111-1111-11111111"), 1, roomCreatedEvent); 
            given.Context.RetrievedEvents.Add(labEvent);
        }

        public static void CreateTheRoom(this IWhen<ReadRoomContext> when)
        {
            ILabEvent labEvent = when.Context.RetrievedEvents[0];
            when.Context.Room = new RoomModel();
            when.Context.Room.ApplyRoomCreatedEvent(labEvent);
        }

        public static void Room(this IGiven<ReadRoomContext> given)
        {
        }

        public static Action<ILabEvent> GetEventHandler(ReadRoomContext context)
        {
            return e => context.RetrievedEvents.Add(e);
        }

        public static void RoomIsCreated(this IThen<ReadRoomContext> then)
        {
            Assert.NotNull(then.Context.Room);
        }

        public static void IdIsSet(this IThen<ReadRoomContext> then)
        {
            Assert.NotNull(then.Context.Room.Id);
            Assert.NotEqual(default(Guid), then.Context.Room.Id);
        }

    }
}