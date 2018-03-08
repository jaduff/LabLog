using System;

namespace LabLog.Domain.Events
{
    public class UpdateDamageTicketEvent: IEventBody
    {
        public UpdateDamageTicketEvent(string roomName, string serialNumber, int damageId, string ticketId)
        {
            RoomName = roomName;
            SerialNumber = serialNumber;
            DamageId = damageId;
            TicketId = ticketId;
        }

        [Obsolete("This constructor is for serialization only. Do not use in code.")]
        public UpdateDamageTicketEvent()
        {

        }

        public string EventType => EventTypeString;
        public int DamageId {get; set;}
        public string TicketId {get; set;}
        public string RoomName {get; set;}
        public string SerialNumber {get; set;}
        public const string EventTypeString="UpdateDamageTicket";

    }
}