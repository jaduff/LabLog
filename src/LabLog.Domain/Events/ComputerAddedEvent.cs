namespace LabLog.Domain.Events
{
    public class ComputerAddedEvent : LabEvent
    {
        public override string EventType => "ComputerAdded";
    }
}