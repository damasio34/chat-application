using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApplication.Core.Domain.Services
{
    public interface IBrockerService
    {
        Task ReceiveMessage(CancellationToken cancelationToken, EventHandler<BasicDeliverEventArgs> onReceiveMessage, string queue);
        void SendMessage<T>(T message, string queue);
    }
}
