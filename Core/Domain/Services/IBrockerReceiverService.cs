using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace ChatApplication.Core.Domain.Services
{
    public interface IBrockerReceiverService
    {
        void ReceiveMessage(CancellationToken stoppingToken, EventHandler<BasicDeliverEventArgs> onReceiveMessage);
    }
}
