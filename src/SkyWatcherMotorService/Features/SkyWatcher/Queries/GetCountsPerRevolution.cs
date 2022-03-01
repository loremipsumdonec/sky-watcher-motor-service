using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetCountsPerRevolution
        : Query
    {
        public GetCountsPerRevolution()
        {
        }

        public GetCountsPerRevolution(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }
    }

    public class GetCountsPerRevolutionModel 
        : IModel
    {
        public long Steps { get; set; }
    }

    [Handle(typeof(GetCountsPerRevolution))]
    public class GetCountsPerRevolutionResultHandler
        : QueryHandler<GetCountsPerRevolution>
    {
        private readonly ISkyWatcherManager _manager;

        public GetCountsPerRevolutionResultHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<IModel> ExecuteAsync(GetCountsPerRevolution query)
        {
            var model = new GetCountsPerRevolutionModel();

            using(var client = _manager.CreateClient(query.MotorId)) 
            {
                model.Steps = await client.SendAndRecieveAsync("a");
            }

            return model;
        }
    }
}
