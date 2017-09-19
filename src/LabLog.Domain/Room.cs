using System;

namespace ClassLogger.Domain
{
    public class Room
    {
        private readonly Action<ILabEvent> _eventHandler;

        public Room(Action<ILabEvent> eventHandler)
        {
            _eventHandler = eventHandler;
        }
        public void AddComputer(Computer computer)
        {
            if (_eventHandler == null)
            {
                return;
            }

            var @event = new ComputerAddedEvent();
            _eventHandler(@event);
        }
    }
}