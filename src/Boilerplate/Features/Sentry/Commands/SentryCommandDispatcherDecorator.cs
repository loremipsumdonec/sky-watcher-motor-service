using Boilerplate.Features.Core.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sentry;

namespace Boilerplate.Features.Sentry.Commands
{
    public class SentryCommandDispatcherDecorator
        : ICommandDispatcher
    {
        private readonly ICommandDispatcher _decorated;

        public SentryCommandDispatcherDecorator(ICommandDispatcher decorated)
        {
            _decorated = decorated;
        }

        public async Task<bool> DispatchAsync(ICommand command)
        {
            var current = SentrySdk.GetSpan().StartChild("command." + command.GetType().Name);

            if (current is null)
            {
                return await _decorated.DispatchAsync(command);
            }

            try
            {
                current.Description = GetAsJson(command);

                return await _decorated.DispatchAsync(command);
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
        private string GetAsJson(ICommand command)
        {
            try
            {
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

                return JsonConvert.SerializeObject(command, settings);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
