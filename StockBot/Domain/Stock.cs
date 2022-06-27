using CsvHelper.Configuration.Attributes;

namespace ChatApplication.StockBot.Domain
{
    public class Stock
    {
        [Name("Close")]
        public string Quote { get; set; }
        [Name("Symbol")]
        public string Code { get; set; }
    }
}
