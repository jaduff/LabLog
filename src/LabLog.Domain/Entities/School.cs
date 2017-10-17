using System;
using System.Collections.Generic;
using LabLog.Domain.Events;
using LabLog.Domain.Exceptions;

namespace LabLog.Domain.Entities
{
    public class School
    {
        private readonly Action<ILabEvent> _eventHandler;

        private School(Action<ILabEvent> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public School(Guid id, List<LabEvent> events, Action<ILabEvent> eventHandler)
        {
            _eventHandler = eventHandler;
            Id = id;
            foreach (LabEvent _event in events)
            {
                Replay(_event);
            }
        }

        public static School Create(string name, Action<ILabEvent> eventHandler)
        {
            if (name == "")
            {
                LabException ex = new LabException("School name can't be blank");
                throw ex;
            }
            var school = new School(eventHandler);
            school.Id = Guid.NewGuid();
            var e = LabEvent.Create(school.Id, 
                ++school.Version, new SchoolCreatedEvent { Name = name });
            school._eventHandler(e);
            return school;
        }

        public List<Computer> Computers { get; } = new List<Computer>();
        public List<Room> Rooms { get; } = new List<Room>();
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
                    new SchoolNameChangedEvent(_name)
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

        public void AddRoom(string roomName)
        {
            if (_eventHandler == null)
            {
                return;
            }

            var @event = LabEvent.Create(
                Guid.NewGuid(),
                ++Version,
                new RoomAddedEvent(roomName));
            _eventHandler(@event);
        }

        private void ApplyComputerAddedEvent(ILabEvent e)
        {
            var body = e.GetEventBody<ComputerAddedEvent>();
            Computers.Add(new Computer(body.ComputerId,
                body.ComputerName));
        }

        private void ApplyRoomAddedEvent(ILabEvent e)
        {
            var body = e.GetEventBody<RoomAddedEvent>();
            Rooms.Add(new Room(body.RoomName));
        }

        private void ApplySchoolCreatedEvent(ILabEvent e)
        {
            Id = e.SchoolId;
            var body = e.GetEventBody<SchoolCreatedEvent>();
            _name = body.Name;
        }

        public void Replay(ILabEvent labEvent)
        {
            switch (labEvent.EventType)
            {
                case SchoolCreatedEvent.EventTypeString:
                    ApplySchoolCreatedEvent(labEvent);
                break;
                case ComputerAddedEvent.EventTypeString:
                    ApplyComputerAddedEvent(labEvent);
                break;
                case RoomAddedEvent.EventTypeString:
                    ApplyRoomAddedEvent(labEvent);
                break;
            }
        }
    }
}