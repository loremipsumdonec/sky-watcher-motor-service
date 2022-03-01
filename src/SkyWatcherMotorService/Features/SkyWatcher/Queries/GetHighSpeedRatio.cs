using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetHighSpeedRatio
        : Query
    {
        public GetHighSpeedRatio()
        {
        }

        public GetHighSpeedRatio(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }
    }

    public class GetHighSpeedRatioModel 
        : IModel
    {
        public long Steps { get; set; }
    }

    [Handle(typeof(GetHighSpeedRatio))]
    public class GetHighSpeedRatioHandler
        : QueryHandler<GetHighSpeedRatio>
    {
        private readonly ISkyWatcherManager _manager;

        public GetHighSpeedRatioHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<IModel> ExecuteAsync(GetHighSpeedRatio query)
        {
            var model = new GetHighSpeedRatioModel();

            using (var client = _manager.CreateClient(query.MotorId))
            {
                model.Steps = await client.SendAndRecieveAsync("g");
            }

            return model;
        }
    }
}
