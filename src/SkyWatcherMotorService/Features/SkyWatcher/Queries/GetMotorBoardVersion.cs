using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetMotorBoardVersion
        : Query
    {
        public GetMotorBoardVersion()
        {
        }

        public GetMotorBoardVersion(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }
    }

    public class GetMotorBoardVersionModel 
        : IModel
    {
        public string Version { get; set; }

        public string Type { get; set; }
    }

    [Handle(typeof(GetMotorBoardVersion))]
    public class GetMotorBoardVersionHandler
        : QueryHandler<GetMotorBoardVersion>
    {
        private readonly ISkyWatcherManager _manager;

        public GetMotorBoardVersionHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<IModel> ExecuteAsync(GetMotorBoardVersion query)
        {
            using (var client = _manager.CreateClient(query.MotorId)) 
            {
                var (total, major, minor, patch) = await client.SendAndReciveSectionsAsync("e");

                return new GetMotorBoardVersionModel()
                {
                    Version = $"{major}.{minor}.{patch:X}"
                };
            }
        }
    }
}
