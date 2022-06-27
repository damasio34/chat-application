using ChatApplication.Core.Domain;
using ChatApplication.Core.Services;
using ChatApplication.StockBot.Services;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace StockBot
{
    class Program
    {        
        static async Task Main(string[] args)
        {
            var brockerService = new BrockerService();
            var cancelationToken = new CancellationToken();
            await brockerService.ReceiveMessage(cancelationToken, ProcessMessage(brockerService), "api-to-bot");
        }

        private static EventHandler<BasicDeliverEventArgs> ProcessMessage(BrockerService brockerService)
        {
            return async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var receivedMessage = JsonSerializer.Deserialize<ChatMessage>(message);                
                var stockQuote = await StockService.GetQuoteFromCSV(receivedMessage.MessageCode.Value);
                var chatMessage = new ChatMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = $"{receivedMessage.MessageCode.Value.ToUpper()} quote is ${stockQuote.Quote} per share",
                    Username = "StockBot",
                    When = DateTime.UtcNow,
                    MessageCode = receivedMessage.MessageCode,
                    ConnectionId = receivedMessage.ConnectionId
                };

                Console.WriteLine(chatMessage.Text);

                brockerService.SendMessage(chatMessage, "bot-to-api");
            };
        }
    }
}
