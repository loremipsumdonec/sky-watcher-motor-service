using Boilerplate.Features.Reactive.Events;
using Boilerplate.Features.Reactive.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Boilerplate.Features.RabbitMQ.Services
{
    public class RabbitMQEventHub
        : IEventHub
    {
        private readonly IEventHub _decorated;

        public RabbitMQEventHub(IEventHub decorated)
        {
            _decorated = decorated;
        }

        public bool IsOpen => throw new NotImplementedException();

        public void Close()
        {
            _decorated.Close();
        }

        public void Connect(Action<IObservable<IEvent>> connect)
        {
            _decorated.Connect(connect);
        }

        public void Connect(Func<IObservable<IEvent>, IDisposable> connect)
        {
            _decorated.Connect(connect);
        }

        public void Dispatch(IEvent @event)
        {
            Publish(@event);
            _decorated.Dispatch(@event);
        }

        public void Open()
        {
            _decorated.Open();
        }

        private void Publish(IEvent @event)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "ACatChasingAWeakToadOnTheFloorInMyHouse"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            string routingKey = @event.GetType().Name;

            channel.BasicPublish(exchange: "micro.events",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
