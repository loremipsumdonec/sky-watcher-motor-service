using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetBreakSteps
        : Query
    {
        public GetBreakSteps()
        {
        }

        public GetBreakSteps(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }
    }

    public class GetBreakStepsModel
        : IModel
    {
        public long Steps { get; set; }
    }

    [Handle(typeof(GetBreakSteps))]
    public class GetBreakStepsHandler
        : QueryHandler<GetBreakSteps>
    {
        private readonly ISkyWatcherManager _manager;

        public GetBreakStepsHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<IModel> ExecuteAsync(GetBreakSteps query)
        {
            var model = new GetBreakStepsModel();

            using (var client = _manager.CreateClient(query.MotorId))
            {
                model.Steps = await client.SendAndRecieveAsync("c");

                if(model.Steps == 0) 
                {
                    model.Steps = 3500;
                }
            }

            return model;
        }
    }
}
