using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LaCorridor.Logging.AzureEventHubs
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LogEntry
    {
        [JsonProperty("logLevel")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel LogLevel { get; set; }

        [JsonProperty("eventId")]
        public EventId EventId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public string Scope { get; set; }

        public LogEntry(LogLevel logLevel, EventId eventId, string message, string category, string scope)
        {
            LogLevel = logLevel;
            EventId = eventId;
            Message = message;
            Category = category;
            Scope = scope;
        }
    }
}
