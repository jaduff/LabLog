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
            given.Context.Id = new Guid("11111111-1111-1111-1111-11111111");
            given.Context.Name = "Test Room";
            given.Context.Version = 1;
            RoomCreatedEvent roomCreatedEvent = new RoomCreatedEvent();
            roomCreatedEvent.Name = given.Context.Name;
            LabEvent labEvent = LabEvent.Create(given.Context.Id, given.Context.Version, roomCreatedEvent); 
            given.Context.RetrievedEvents.Add(labEvent);
        }

        public static void RoomIdMatches(this IThen<ReadRoomContext> then)
        {
            Assert.Equal(then.Context.Room.Id,then.Context.Id);
        }
        public static void RoomVersionMatches(this IThen<ReadRoomContext> then)
        {
            Assert.Equal(then.Context.Room.Version,then.Context.Version);
        }
        public static void RoomNameMatches(this IThen<ReadRoomContext> then)
        {
            Assert.Equal(then.Context.Room.Name,then.Context.Name);

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