using ChatApplication.StockBot.Domain;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatApplication.StockBot.Services
{
    public class StockService
    {
        private static readonly HttpClient client = new();

        public static async Task<Stock> GetQuoteFromCSV(string stockCode)
        {
            var stockStream = await GetStockQuoteAsync(stockCode.ToLower());
            using var reader = new StreamReader(stockStream);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            var stockQuote = csvReader.GetRecord<Stock>();
            return stockQuote;
        }

        private static Task<Stream> GetStockQuoteAsync(string stockCode)
        {
            var streamTask = client.GetStreamAsync(@$"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
            return streamTask;
        }
    }
}
