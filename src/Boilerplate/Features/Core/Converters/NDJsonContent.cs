using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;

namespace Boilerplate.Features.Core.Converters
{
    public class NDJsonContent
        : JsonContent
    {
        public NDJsonContent(object content)
            : this(content, new CamelCasePropertyNamesContractResolver())
        {
        }

        public NDJsonContent(object content, IContractResolver contractResolver)
            : base(content, contractResolver)
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/x-ndjson");
        }

        protected override JsonSerializer CreateJsonSerializer()
        {
            JsonSerializer serializer = base.CreateJsonSerializer();
            serializer.Converters.Add(new NDJsonConverter());

            return serializer;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}
