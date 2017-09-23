using Newtonsoft.Json;

namespace LabLog.Domain.Events
{
    public interface IEventBody
    {
        [JsonIgnore]
        string EventType{get;}
    }
}