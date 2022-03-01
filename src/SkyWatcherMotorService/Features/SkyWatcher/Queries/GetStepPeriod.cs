using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetStepPeriod
        : Query
    {
        public GetStepPeriod()
        {
        }

        public GetStepPeriod(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }
    }

    public class GetStepPeriodModel
        : IModel
    {
        public long Steps { get; set; }
    }

    [Handle(typeof(GetStepPeriod))]
    public class GetStepPeriodHandler
        : QueryHandler<GetStepPeriod>
    {
        private readonly ISkyWatcherManager _manager;

        public GetStepPeriodHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<IModel> ExecuteAsync(GetStepPeriod query)
        {
            var model = new GetStepPeriodModel();

            using (var client = _manager.CreateClient(query.MotorId))
            {
                model.Steps = await client.SendAndRecieveAsync("i");
            }

            return model;
        }
    }
}
