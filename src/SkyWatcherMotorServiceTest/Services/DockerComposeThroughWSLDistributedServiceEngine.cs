using Boilerplate.Features.Testing.Services;
using CliWrap;
using CliWrap.Buffered;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SkyWatcherMotorServiceTest.Services
{
    internal class DockerComposeThroughWSLDistributedServiceEngine
        : DistributedServiceEngine
    {
        private readonly string _dockerComposeDirectory;

        public DockerComposeThroughWSLDistributedServiceEngine(string dockerComposeDirectory, IReadinessProbe readinessProbe)
            : base(readinessProbe)
        {
            Assert.True(
                File.Exists(Path.Combine(dockerComposeDirectory, "docker-compose.yml")
            ), "docker-compose.yml file does not exists");

            _dockerComposeDirectory = dockerComposeDirectory;
        }

        public override async Task StartAsync()
        {
            await Cli.Wrap("wsl")
                .WithWorkingDirectory(_dockerComposeDirectory)
                .WithArguments($"docker compose up -d")
                .ExecuteBufferedAsync();

            await base.StartAsync();
        }

        public override async Task StopAsync()
        {
            await Cli.Wrap("wsl")
                .WithWorkingDirectory(_dockerComposeDirectory)
                .WithArguments($"docker compose down")
                .ExecuteBufferedAsync();
        }
    }
}
