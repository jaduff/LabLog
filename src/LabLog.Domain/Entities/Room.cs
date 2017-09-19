using System;
using System.Collections.Generic;
using LabLog.Domain.Events;

namespace LabLog.Domain.Entities
{
    public class Room
    {
        private readonly Action<ILabEvent> _eventHandler;

        public Room(Action<ILabEvent> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public List<Computer> Computers { get; } = new List<Computer>();

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