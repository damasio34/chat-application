using ChatApplication.StockBot.Domain;
using ChatApplication.StockBot.Services;
using FluentAssertions;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;

namespace StockBot.Test
{
    public class StockQuoteServiceTest
    {
        [Fact]
        [Category("bot_that_will_call_an_API_using_the_stock_code_as_a_parameter")]
        public async Task Get_StockQuoteAsync_Shold_Return_Stock_Quote()
        {
            const string stockCode = "AAPL.US";
            var stock = await StockService.GetQuoteFromCSV(stockCode);

            stock.Should().BeOfType<Stock>();
            stock.Code.ToLower().Should().Be(stockCode.ToLower());
            stock.Quote.Should().NotBe("N/D");
        }
    }
}
