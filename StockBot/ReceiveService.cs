using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace ChatApplication.StockBot
{
    public class ReceiveService
    {
        public void ReceiveMessage(EventHandler<BasicDeliverEventArgs> onReceiveMessage)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "bot",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += onReceiveMessage;
                channel.BasicConsume(queue: "bot",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Wainting For Message");
                Console.ReadKey();
            }
        }
    }
}
