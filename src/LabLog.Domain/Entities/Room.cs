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
            var e = LabEvent.Create(room.Id, 
                ++room.Version, new RoomCreatedEvent());
            e.SetEventBody(new RoomCreatedEvent());
            room._eventHandler(e);
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
                var @event = LabEvent.Create(Id,
                    ++Version,
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

            var @event = LabEvent.Create(
                Guid.NewGuid(),
                ++Version,
                new ComputerAddedEvent(computer.ComputerId, computer.ComputerName));
            _eventHandler(@event);
        }

        private void ApplyComputerAddedEvent(ILabEvent e)
        {
            var body = e.GetEventBody<ComputerAddedEvent>();
            Computers.Add(new Computer(body.ComputerId,
                body.ComputerName));
        }

        public void Replay(ILabEvent labEvent)
        {
            switch (labEvent.EventType)
            {
                case ComputerAddedEvent.EventTypeString:
                    ApplyComputerAddedEvent(labEvent);
                break;
            }
        }
    }
}