using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetPosition
        : Query
    {
        public GetPosition()
        {
        }

        public GetPosition(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }
    }

    public class GetPositionModel 
        : IModel
    {
        public long Steps { get; set; }

        public double Degree { get; set; }
    }

    [Handle(typeof(GetPosition))]
    public class GetPositionHandler
        : QueryHandler<GetPosition>
    {
        private readonly ISkyWatcherManager _manager;

        public GetPositionHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<IModel> ExecuteAsync(GetPosition query)
        {
            var model = new GetPositionModel();

            var motor = _manager.GetMotor(query.MotorId);

            using (var client = _manager.CreateClient(query.MotorId))
            {
                model.Steps = await client.SendAndRecieveAsync("j") - 0x00800000;
                model.Degree = motor.ConvertStepsToDegreee(model.Steps);
            }

            return model;
        }
    }
}
