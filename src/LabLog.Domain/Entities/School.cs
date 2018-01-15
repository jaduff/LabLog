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

        public School(List<LabEvent> events, Action<ILabEvent> eventHandler)
        {
            _eventHandler = eventHandler;
            foreach (LabEvent _event in events)
            {
                Replay(_event);
            }
        }

        public static School Create(string name, Action<ILabEvent> eventHandler)
        {
            if (name == "")
            {
                LabException ex = new LabException();
                ex.AddException(new Exception("School names must not be null"));
                throw ex;
            }
            var school = new School(eventHandler);
            school.Id = Guid.NewGuid();
            var e = LabEvent.Create(school.Id, 
                ++school.Version, new SchoolCreatedEvent { Name = name });
            school._eventHandler(e);
            return school;
        }

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

        public void AddComputer(Guid roomId, Computer computer)
        {
            if (_eventHandler == null)
            {
                return;
            }

            LabException ex = new LabException();
            foreach (Room _room in Rooms)
            {
                if (_room.Computers.FindAll(f => (f.SerialNumber == computer.SerialNumber)).Count > 0)
                {
                    ex.AddException(new UniqueComputerSerialException());
                }
            }

            Room room = Rooms.Find(f => (f.RoomId == roomId));

            foreach (Computer _computer in room.Computers)
            {
                if (computer.Position == _computer.Position) { ex.AddException(new UniqueComputerRoomPositionException());}
            }

            if (ex.HasExceptions()) {throw ex;}

            var @event = LabEvent.Create(
                //Guid.NewGuid(),
                Id,
                ++Version,
                new ComputerAddedEvent(roomId, computer.SerialNumber, computer.ComputerName, computer.Position));
                ApplyComputerAddedEvent(@event);
            _eventHandler(@event);
        }

        public void AddRoom(string roomName)
        {
            if (_eventHandler == null)
            {
                return;
            }

            if (Rooms.FindAll(f => (f.RoomName == roomName)).Count > 0)
            {
                var exceptions = new LabException();
                exceptions.AddException(new UniqueRoomNameException());
                throw exceptions;
            }

            var @event = LabEvent.Create(
                Id,
                ++Version,
                new RoomAddedEvent(Guid.NewGuid(), roomName));
            ApplyRoomAddedEvent(@event);
            _eventHandler(@event);
        }

        public void AssignStudent(string username, string serialNumber)
        {
            if (_eventHandler == null)
            {
                return;
            }

            var @event = LabEvent.Create(Id, ++Version,
                new StudentAssignedEvent(serialNumber, username));
            ApplyStudentAssignedEvent(@event);
            _eventHandler(@event);
        }

        public void RecordDamage(string roomName, string serialNumber, string damageDescription)
        {
            if (_eventHandler == null)
            {
                return;
            }

            Computer computer = GetComputerBySerial(serialNumber);
            var @event = LabEvent.Create(Id, ++Version,
                    new DamageAddedEvent(roomName, serialNumber, computer.GetLastDamageId() + 1, damageDescription));
            ApplyDamageAddedEvent(@event);
            _eventHandler(@event);
        }

        private void ApplyDamageAddedEvent(ILabEvent e)
        {
            var body = e.GetEventBody<DamageAddedEvent>();
            Computer computer = GetComputerBySerial(body.SerialNumber);
            Damage damage = new Damage(computer.GetLastDamageId() + 1, body.DamageDescription);
            computer.DamageList.Add(damage);
            Version = e.Version;
        }

        private void ApplyComputerAddedEvent(ILabEvent e)
        {
            var body = e.GetEventBody<ComputerAddedEvent>();
            Room room = Rooms.Find(f => (f.RoomId == body.RoomId));
            if (room == null) { throw new Exception ("Could not find room with id " + body.RoomId + ".  Found " + Rooms.Count + " rooms.");}
            room.Computers.Add(new Computer(body.SerialNumber,
                body.ComputerName, body.Position));
            if (room.Computers == null) {throw new Exception("room.Computers is null");}
            Version = e.Version;
        }

        private void ApplyStudentAssignedEvent(ILabEvent e)
        {
            var body = e.GetEventBody<StudentAssignedEvent>();
            Version = e.Version; 
        }
 
        private void ApplyRoomAddedEvent(ILabEvent e)
        {
            var body = e.GetEventBody<RoomAddedEvent>();
            Room room = new Room(body.RoomId, body.RoomName);
            Rooms.Add(room);
            Version = e.Version;
        }

        private void ApplySchoolCreatedEvent(ILabEvent e)
        {
            Id = e.SchoolId;
            var body = e.GetEventBody<SchoolCreatedEvent>();
            _name = body.Name;
            Version = e.Version;
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
                case StudentAssignedEvent.EventTypeString:
                    ApplyStudentAssignedEvent(labEvent);
                break;
            }
        }

        private Computer GetComputerBySerial(string serialNumber)
        {
            foreach (Room room in Rooms)
            {
                Computer _computer = room.Computers.Find(f => f.SerialNumber == serialNumber);
                if (_computer.SerialNumber == serialNumber) { return _computer; }
            }
            throw new Exception("No computer found");
        }
    }
}