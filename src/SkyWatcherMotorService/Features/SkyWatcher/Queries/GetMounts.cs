using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Models;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Queries
{
    public class GetMounts
        : Query
    {
    }

    public class GetMountsModel 
        : IModel
    {
        public IEnumerable<Mount> Mounts { get; set; } = new List<Mount>();

        public void Add(IEnumerable<Mount> mounts) 
        {
            ((List<Mount>)Mounts).AddRange(mounts);
        }
    }

    [Handle(typeof(GetMounts))]
    public class GetMountsHandler
        : QueryHandler<GetMounts>
    {
        private readonly ISkyWatcherManager _manager;

        public GetMountsHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override Task<IModel> ExecuteAsync(GetMounts query)
        {
            var model = new GetMountsModel();
            model.Add(_manager.GetMounts());

            return Task.FromResult((IModel)model);
        }
    }
}
