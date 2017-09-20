using System;
using System.Collections.Generic;
using LabLog.Domain.Events;

namespace LabLog.Domain.Entities
{
    public class Room
    {
        private readonly Action<ILabEvent> _eventHandler;

        private Room(Action<ILabEvent> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public static Room Create(Action<ILabEvent> eventHandler)
        {
            var room = new Room(eventHandler);
            room.Id = Guid.NewGuid();
            room._eventHandler(new RoomCreatedEvent(room.Id));
            return room;
        }

        public List<Computer> Computers { get; } = new List<Computer>();
        public Guid Id { get; set; }

        public void AddComputer(Computer computer)
        {
            if (_eventHandler == null)
            {
                return;
            }

            var @event = new ComputerAddedEvent();
            _eventHandler(@event);
        }

        public void Replay(ILabEvent labEvent)
        {
            Computers.Add(new Computer(0));
        }
    }
}