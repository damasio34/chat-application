using ChatApplication.API.Hubs;
using ChatApplication.Core.Domain;
using ChatApplication.Core.Domain.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApplication.API.HostedServices
{
    public class BrockerScopedHostedService : BackgroundService
    {
        private readonly ILogger<BrockerScopedHostedService> _logger;
        private readonly IHubContext<ChatHub> _hubContext;

        public BrockerScopedHostedService(IServiceProvider services,
           IHubContext<ChatHub> hubContext,
           ILogger<BrockerScopedHostedService> logger)
        {
            Services = services;
            _logger = logger;
            _hubContext = hubContext;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            using var scope = Services.CreateScope();
            var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IBrockerService>();
            await scopedProcessingService.ReceiveMessage(stoppingToken, async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var chatMessage = JsonSerializer.Deserialize<ChatMessage>(message);
                _logger.LogInformation(message);
                await this._hubContext.Clients.Client(chatMessage.ConnectionId)?.SendAsync("receiveMessage", chatMessage);
            }, "bot-to-api");            
        }
    }
}
