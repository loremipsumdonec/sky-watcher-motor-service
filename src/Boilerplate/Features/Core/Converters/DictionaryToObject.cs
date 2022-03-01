using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boilerplate.Features.Core.Converters
{
    public class DictionaryToObject
        : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Dictionary<string, object>).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dictionary = (Dictionary<string, object>)value;
            var root = new JObject();

            foreach (var keyValue in dictionary)
            {
                string[] steps = keyValue.Key.Split('.');
                var current = root;

                for (int index = 0; index < steps.Length; index++)
                {
                    var step = steps[index];
                    bool isLastStep = index == steps.Length - 1;

                    var property = current.GetValue(step);

                    if (property == null)
                    {
                        if (isLastStep)
                        {
                            current.Add(new JProperty(step, keyValue.Value));
                        }
                        else
                        {
                            var propertyValue = new JObject();
                            current.Add(new JProperty(step, propertyValue));
                            current = propertyValue;
                        }
                    }
                    else
                    {
                        current = (JObject)property;
                    }
                }
            }

            root.WriteTo(writer);
        }
    }
}