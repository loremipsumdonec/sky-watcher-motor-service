using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http.Headers;

namespace Boilerplate.Features.Core.Converters
{
    public class JsonContent
        : HttpContent
    {
        private readonly object _content;
        private readonly IContractResolver _contractResolver;

        public JsonContent(object content)
            : this(content, new CamelCasePropertyNamesContractResolver())
        {
        }

        public JsonContent(object content, IContractResolver contractResolver)
        {
            _content = content;
            _contractResolver = contractResolver;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            JsonTextWriter writer = new JsonTextWriter(new StreamWriter(stream));
            JsonSerializer serializer = CreateJsonSerializer();

            serializer.Serialize(writer, _content);
            writer.Flush();

            return Task.FromResult<object>(null);
        }

        protected virtual JsonSerializer CreateJsonSerializer()
        {
            return new JsonSerializer()
            {
                ContractResolver = _contractResolver
            };
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}
