using Boilerplate.Features.Testing.Services;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Boilerplate.Features.RabbitMQ.Services
{
    public class RabbitMQReadinessProbe
        : ReadinessProbe
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _host;

        public RabbitMQReadinessProbe(IConfigurationSection section) 
            : this(
                  section.GetSection("username").Value,
                  section.GetSection("password").Value,
                  section.GetSection("host").Value
            )
        { 
        }

        public RabbitMQReadinessProbe(
            string username, 
            string password, 
            string host
        )
        {
            _username = username;
            _password = password;
            _host = host;
        }

        protected override Task<bool> IsReadyAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _host,
                UserName = _username,
                Password = _password
            };

            try
            {
                using var connection = factory.CreateConnection();
                var model = connection.CreateModel();
                model.Close();
                connection.Close();

                return Task.FromResult(true);
            }
            catch
            {
            }

            return Task.FromResult(false);
        }
    }
}
