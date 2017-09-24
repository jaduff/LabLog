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
            _eventHandler = e =>
            {
                Version++;
                eventHandler(e);
            };
        }

        public static Room Create(Action<ILabEvent> eventHandler)
        {
            var room = new Room(eventHandler);
            room.Id = Guid.NewGuid();
            room._eventHandler(new LabEvent<RoomCreatedEvent>(room.Id, new RoomCreatedEvent()));
            return room;
        }

        public List<Computer> Computers { get; } = new List<Computer>();
        public Guid Id { get; set; }
        private String _name;
        public String Name {
            get { return _name; }
            set
            {
                if (_eventHandler == null)
                {
                    return;
                }
                _name = value;
                var @event = new LabEvent<RoomNameChangedEvent>(Id,
                    new RoomNameChangedEvent(_name)
                );
                _eventHandler(@event);
            }
        }

        public int Version { get; private set; }

        public void AddComputer(Computer computer)
        {
            if (_eventHandler == null)
            {
                return;
            }

            var @event = new LabEvent<ComputerAddedEvent>(
                Guid.NewGuid(), 
                new ComputerAddedEvent(computer.ComputerId, computer.ComputerName));
            _eventHandler(@event);
        }

        private void Apply(LabEvent<ComputerAddedEvent> e)
        {
            var body = e.EventBodyObject;

            Computers.Add(new Computer(body.ComputerId,
                body.ComputerName));
        }

        public void Replay(ILabEvent labEvent)
        {
            Apply((LabEvent<ComputerAddedEvent>) labEvent);
        }
    }
}