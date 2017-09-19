namespace ClassLogger.Domain
{
    public class ComputerAddedEvent : LabEvent
    {
        public override string EventType => "ComputerAdded";
    }
}