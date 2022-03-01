using Autofac;
using Boilerplate.Features.Core.Config;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher
{
    public class SkyWatcherModule
        : Module
    {
        public SkyWatcherModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterFromAs<ISkyWatcherManager>(
                "sky.watcher.manager",
                Configuration
            ).SingleInstance();
        }
    }
}
