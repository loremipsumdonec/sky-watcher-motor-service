using Boilerplate.Features.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Boilerplate.Features.Reactive.Events
{
    public class Heartbeat
        : Event, IExtendableModel
    {
        public Heartbeat(string message)
        {
            Message = message;
            Timestamp = DateTime.Now;
            Severity = Severitys.Info;
            Extensions = new Dictionary<string, object>();
        }

        public Heartbeat(string message, Severitys severity)
        {
            Message = message;
            Timestamp = DateTime.Now;
            Severity = severity;

            Extensions = new Dictionary<string, object>();
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Severitys Severity { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        [JsonProperty("@timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; set; }

        public void AddExtension(string key, object model)
        {
            Extensions.Add(key, model);
        }
    }
}
