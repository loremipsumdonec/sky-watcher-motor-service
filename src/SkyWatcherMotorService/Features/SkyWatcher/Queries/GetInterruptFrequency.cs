using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetInterruptFrequency
        : Query
    {
        public GetInterruptFrequency()
        {
        }

        public GetInterruptFrequency(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }
    }

    public class GetInterruptFrequencyModel
        : IModel
    {
        public long Steps { get; set; }
    }

    [Handle(typeof(GetInterruptFrequency))]
    public class GetInterruptFrequencyHandler
        : QueryHandler<GetInterruptFrequency>
    {
        private readonly ISkyWatcherManager _manager;

        public GetInterruptFrequencyHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<IModel> ExecuteAsync(GetInterruptFrequency query)
        {
            var model = new GetInterruptFrequencyModel();

            using (var client = _manager.CreateClient(query.MotorId))
            {
                model.Steps = await client.SendAndRecieveAsync("b");
            }

            return model;
        }
    }
}
