using ChatApplication.StockBot.Domain;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatApplication.StockBot.Services
{
    public class StockQuoteService
    {
        private static readonly HttpClient client = new HttpClient();

        public static StockQuote GetQuoteFromCSV(Stream stockStream)
        {
            using var reader = new StreamReader(stockStream);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            var stockQuote = csvReader.GetRecord<StockQuote>();
            return stockQuote;
        }

        public static Task<Stream> GetStockQuoteAsync(string stockCode)
        {
            var streamTask = client.GetStreamAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
            return streamTask;
        }
    }
}
