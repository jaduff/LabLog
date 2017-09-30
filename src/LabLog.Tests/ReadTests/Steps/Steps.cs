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
        public static void RoomCreatedEvent(this IGiven<ReadRoomContext> given, Guid guid, int version, string name)
        {
            RoomCreatedEvent roomCreatedEvent = new RoomCreatedEvent();
            roomCreatedEvent.Name = name;
            LabEvent labEvent = LabEvent.Create(guid, version, roomCreatedEvent); 
            given.Context.RetrievedEvents.Add(labEvent);
        }

        public static void RoomIdMatches(this IThen<ReadRoomContext> then, Guid guid)
        {
            Assert.Equal(then.Context.Room.Id,guid);
        }
        public static void RoomVersionMatches(this IThen<ReadRoomContext> then, int version)
        {
            Assert.Equal(then.Context.Room.Version,version);
        }
        public static void RoomNameMatches(this IThen<ReadRoomContext> then,string name)
        {
            Assert.Equal(then.Context.Room.Name,name);

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