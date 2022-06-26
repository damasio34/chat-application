using ChatApplication.StockBot;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockBot
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            var receiveService = new ReceiveService();
            var sendService = new SendService();

            receiveService.ReceiveMessage(async (model, ea) =>
            {
                var stockQuoteService = new StockQuoteService();
                var body = ea.Body.ToArray();
                var stockCode = Encoding.UTF8.GetString(body);
                //var stockCode = "aapl.us";
                var stockStream = await GetStockQuoteAsync(stockCode);
                var stockQuote = stockQuoteService.GetQuoteFromCSV(stockStream);

                Console.WriteLine(stockQuote.Code);
                Console.WriteLine(stockQuote.Quote);

                sendService.SendMessage(stockQuote.Quote.ToString());
            });

            //var stockCode = "aapl.us";
            //var stockStream = await GetStockQuoteAsync(stockCode);
            //var stockQuote = stockQuoteService.GetQuoteFromCSV(stockStream);

            //Console.WriteLine(stockQuote.Code);
            //Console.WriteLine(stockQuote.Quote);
            Console.ReadKey();
        }
        private static Task<Stream> GetStockQuoteAsync(string stockCode)
        {
            var streamTask = client.GetStreamAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
            return streamTask;
        }
    }
}
