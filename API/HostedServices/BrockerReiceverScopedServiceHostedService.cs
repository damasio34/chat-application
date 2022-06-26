using ChatApplication.API.Hubs;
using ChatApplication.Core.Domain;
using ChatApplication.Core.Domain.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApplication.API.HostedServices
{
    public class BrockerReiceverScopedServiceHostedService : BackgroundService
    {
        private readonly ILogger<BrockerReiceverScopedServiceHostedService> _logger;
        private readonly IHubContext<ChatHub> _hubContext;

        public BrockerReiceverScopedServiceHostedService(IServiceProvider services,
           IHubContext<ChatHub> hubContext,
           ILogger<BrockerReiceverScopedServiceHostedService> logger)
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

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IBrockerReceiverService>();

                scopedProcessingService.ReceiveMessage(stoppingToken, async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var stockQuote = Encoding.UTF8.GetString(body);
                    var stockCode = "APPL.US";

                    Console.WriteLine(stockQuote);

                    await this._hubContext.Clients.Group("aapl.us").SendAsync("receiveMessage",
                        new Message
                        {
                            Id = Guid.NewGuid().ToString(),
                            Username = "Bot",
                            When = DateTime.UtcNow,
                            Room = stockQuote,
                            Text = $"{stockCode} quote is ${stockQuote} per share"
                        }
                    );
                });
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
