using ChatApplication.Core.Domain.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace ChatApplication.Core.Services
{
    public class BrockerReceiverService : IBrockerReceiverService
    {
        //private int executionCount = 0;
        //private readonly ILogger _logger;

        //public BrockerReceiverService (ILogger<ScopedProcessingService> logger)
        //{
        //    _logger = logger;
        //}

        public void ReceiveMessage(CancellationToken stoppingToken, EventHandler<BasicDeliverEventArgs> onReceiveMessage)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            
                channel.QueueDeclare(queue: "api",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += onReceiveMessage;
                channel.BasicConsume(queue: "api",
                                     autoAck: true,
                                     consumer: consumer);
        }
    }
}
