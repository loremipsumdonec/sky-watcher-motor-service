using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetStatus
        : Query
    {
        public GetStatus(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }
    }

    public class GetStatusModel
        : IModel
    {
        public bool IsSlewing { get; set; }

        public bool IsTracking { get; set; }

        public bool IsSlewingClockWise { get; set; }

        public bool IsSlewingInHighSpeed { get; set; }

        public bool IsBlocked { get; set; }

        public bool Initialized { get; set; }
    }

    [Handle(typeof(GetStatus))]
    public class GetStatusHandler
        : QueryHandler<GetStatus>
    {
        private readonly ISkyWatcherManager _manager;

        public GetStatusHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<IModel> ExecuteAsync(GetStatus query)
        {
            var model = new GetStatusModel();

            using (var client = _manager.CreateClient(query.MotorId))
            {
                var (_, a, b, c) = await client.SendAndReciveSectionsAsync("f");

                model.IsSlewing = (b & 1) == 1;
                model.IsBlocked = (b >> 1 & 1) == 1;
                model.Initialized = (c & 1) == 1;

                if (model.IsSlewing) 
                {
                    model.IsTracking = (a & 1) == 1;
                    model.IsSlewingClockWise = (a >> 1 & 1) == 0;
                    model.IsSlewingInHighSpeed = (a >> 2 & 1) == 1;
                }
            }

            return model;
        }
    }
}
