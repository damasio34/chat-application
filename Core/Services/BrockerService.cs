using ChatApplication.Core.Domain.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApplication.Core.Services
{
    public class BrockerService : IBrockerService
    {
        private readonly string _hostname;

        public BrockerService()
        {
            this._hostname = "localhost"; // ToDo: Change to Environment Variable
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory() { HostName = this._hostname };
            var connection = factory.CreateConnection();

            return connection;
        }

        public async Task ReceiveMessage(CancellationToken cancelationToken, EventHandler<BasicDeliverEventArgs> onReceiveMessage, string queue)
        {
            using var connection = this.GetConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += onReceiveMessage;
            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);

            while (!cancelationToken.IsCancellationRequested)
            {
                await Task.Delay(10000, cancelationToken);
            }
        }

        public void SendMessage<T>(T message, string queue)
        {
            using var connection = this.GetConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var stringMessage = message is not string ? JsonSerializer.Serialize(message) : message.ToString();
            var body = Encoding.UTF8.GetBytes(stringMessage);
            channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
        }
    }
}
