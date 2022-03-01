using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using Boilerplate.Features.Sentry.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sentry;

namespace Boilerplate.Features.Sentry.Queries
{
    public class SentryQueryDispatcherDecorator
        : IQueryDispatcher
    {
        private readonly IQueryDispatcher _decorated;

        public SentryQueryDispatcherDecorator(IQueryDispatcher decorated)
        {
            _decorated = decorated;
        }

        public async Task<M> DispatchAsync<M>(IQuery query) where M : class, IModel
        {
            var current = SentrySdk.GetSpan()?.StartChild("query." + query.GetType().Name);

            if (current is null)
            {
                return await _decorated.DispatchAsync<M>(query);
            }

            try
            {
                current.Description = GetAsJson(query);

                return await _decorated.DispatchAsync<M>(query);
            }
            catch (Exception ex)
            {
                current.Finish(ex);
                throw;
            }
            finally
            {
                current.Finish(SpanStatus.Ok);
            }
        }

        public async Task<IModel> DispatchAsync(IQuery query)
        {
            var current = SentrySdk.GetSpan().StartChild("query." + query.GetType().Name);

            if (current is null)
            {
                return await _decorated.DispatchAsync(query);
            }

            try
            {
                current.Description = GetAsJson(query);

                return await _decorated.DispatchAsync(query);
            }
            catch (Exception ex)
            {
                current.Finish(ex);
                throw;
            }
            finally
            {
                current.Finish(SpanStatus.Ok);
            }
        }

        private string GetAsJson(IQuery query)
        {
            try
            {
                if (query is IHasSensitiveInformation)
                {
                    return "{ \"message\": \"query has sensitive information and will not be serialized\" }";
                }

                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy
                        {
                            ProcessExtensionDataNames = true
                        }
                    }
                };

                return JsonConvert.SerializeObject(query, settings);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
