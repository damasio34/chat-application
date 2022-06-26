using ChatApplication.StockBot;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockBot
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            var stockQuoteService = new StockQuoteService();

            var stockCode = "aapl.us";
            var stockStream = await GetStockQuoteAsync(stockCode);
            var stockQuote = stockQuoteService.GetQuoteFromCSV(stockStream);

            Console.WriteLine(stockQuote.Code);
            Console.WriteLine(stockQuote.Quote);
            Console.ReadKey();

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{

            //}        
        }
        private static Task<Stream> GetStockQuoteAsync(string stockCode)
        {
            var streamTask = client.GetStreamAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
            return streamTask;
        }
    }
}
