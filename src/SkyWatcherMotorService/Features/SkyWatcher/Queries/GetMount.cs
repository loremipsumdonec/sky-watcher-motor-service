using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetMount
        : Query
    {
        public GetMount(string mountId)
        {
            MountId = mountId;
        }

        public string MountId { get; set; }
    }

    [Handle(typeof(GetMount))]
    public class GetMountHandler
        : QueryHandler<GetMount>
    {
        private readonly ISkyWatcherManager _manager;

        public GetMountHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override Task<IModel> ExecuteAsync(GetMount query)
        {
            var mount = _manager.GetMount(query.MountId);
            return Task.FromResult((IModel)mount);
        }
    }
}
