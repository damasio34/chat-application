using CsvHelper;
using System.Globalization;
using System.IO;

namespace ChatApplication.StockBot
{
    public class StockQuoteService
    {        
        public StockQuoteDTO GetQuoteFromCSV(Stream stockStream)
        {
            using (var reader = new StreamReader(stockStream))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                var stockQuote = csvReader.GetRecord<StockQuoteDTO>();
                return stockQuote;
            }
        }
    }
}
