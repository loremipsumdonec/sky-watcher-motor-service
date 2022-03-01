using Newtonsoft.Json;
using System.Collections;

namespace Boilerplate.Features.Core.Converters
{
    public class NDJsonConverter
        : JsonConverter
    {
        private bool _started;

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return !_started;
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            return null;
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            _started = true;

            if (IsEnumerable(value))
            {
                IEnumerable enumerable = (IEnumerable)value;

                foreach (var item in enumerable)
                {
                    string json = WriteItem(item, serializer);
                    writer.WriteRaw(json);
                    writer.WriteRaw("\n");
                }
            }
            else
            {
                string json = WriteItem(value, serializer);
                writer.WriteRaw(json);
            }
        }

        private string WriteItem(object item, JsonSerializer serializer)
        {
            StringWriter stringWriter = new StringWriter();

            using var itemWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.None };
            serializer.Serialize(itemWriter, item);
            return stringWriter.ToString();
        }

        private bool IsEnumerable(object value)
        {
            return IsEnumerable(value.GetType());
        }

        private bool IsEnumerable(Type objectType)
        {
            return
                (
                    typeof(IEnumerable).IsAssignableFrom(objectType)
                    || typeof(IEnumerable<>).IsAssignableFrom(objectType)
                );
        }
        /*
        
        public static void ToNewlineDelimitedJson<T>(TextWriter textWriter, IEnumerable<T> items)
    {
        var serializer = JsonSerializer.CreateDefault();

        foreach (var item in items)
        {
            // Formatting.None is the default; I set it here for clarity.
            using (var writer = new JsonTextWriter(textWriter) { Formatting = Formatting.None, CloseOutput = false })
            {
                serializer.Serialize(writer, item);
            }
            // https://web.archive.org/web/20180513150745/http://specs.okfnlabs.org/ndjson/
            // Each JSON text MUST conform to the [RFC7159] standard and MUST be written to the stream followed by the newline character \n (0x0A). 
            // The newline charater MAY be preceeded by a carriage return \r (0x0D). The JSON texts MUST NOT contain newlines or carriage returns.
            textWriter.Write("\n");
        }
    }
        

         * */
    }
}
