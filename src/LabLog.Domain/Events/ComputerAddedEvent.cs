namespace LabLog.Domain.Events
{
    public class ComputerAddedEvent : IEventBody
    {
        public string EventType => "ComputerAdded";
    }
}