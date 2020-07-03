using IO.ApiClient.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TradingServer.Database_Connector;

namespace TradingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // GET: api/Orders
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(StaticStorage.MarketOrders);
        }

        // POST: api/Orders
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {

            if (order.Side == SideEnum.Buy)
            {
                var sellOrders = StaticStorage.MarketOrders.Where(marketOrder => marketOrder.Side == SideEnum.Sell);
                var mktSellOrders = sellOrders.Where(marketOrder => order.UserId != marketOrder.UserId);
                var availableOrders = mktSellOrders.Where(marketOrder => marketOrder.Price >= order.Price);


                foreach (var availableOrder in availableOrders)
                {
                    if (order.Quantity != 0 && availableOrder.Quantity != 0)
                    {
                        if (availableOrder.Quantity >= order.Quantity)
                        {
                            availableOrder.Quantity = availableOrder.Quantity - order.Quantity;

                            if ((order.Quantity != 0) || (availableOrder.Quantity != 0))
                                StaticStorage.TradeHistory.Add(new Trade(order.UserId, availableOrder.UserId, order.Symbol, order.Quantity, availableOrder.Price));

                            break;
                        }
                        else
                        {
                            order.Quantity = order.Quantity - availableOrder.Quantity;

                            StaticStorage.TradeHistory.Add(new Trade(order.UserId, availableOrder.UserId, order.Symbol, availableOrder.Quantity, availableOrder.Price));

                            availableOrder.Quantity = 0;
                        }
                    }

                    //StaticStorage.MarketOrders.Remove(availableOrder);
                }

            }

            if (order.Side == SideEnum.Sell)
            {
                var sellOrders = StaticStorage.MarketOrders.Where(marketOrder => marketOrder.Side == SideEnum.Buy);
                var mktSellOrders = sellOrders.Where(marketOrder => order.UserId != marketOrder.UserId);
                var availableOrders = mktSellOrders.Where(marketOrder => marketOrder.Price <= order.Price);

                foreach (var availableOrder in availableOrders)
                {
                    if (order.Quantity != 0 && availableOrder.Quantity != 0)
                    {
                        if (availableOrder.Quantity >= order.Quantity)
                        {
                            availableOrder.Quantity = availableOrder.Quantity - order.Quantity;

                            StaticStorage.TradeHistory.Add(new Trade(availableOrder.UserId, order.UserId, order.Symbol, order.Quantity, availableOrder.Price));

                            break;
                        }
                        else
                        {
                            order.Quantity = order.Quantity - availableOrder.Quantity;

                            StaticStorage.TradeHistory.Add(new Trade(availableOrder.UserId, order.UserId, order.Symbol, availableOrder.Quantity, availableOrder.Price));

                            availableOrder.Quantity = 0;
                        }
                    }

                    //StaticStorage.MarketOrders.Remove(availableOrder);
                }
            }


            StaticStorage.MarketOrders.Add(order);
            return Ok(order);
        }

        // POST: api/Orders/multiple
        [HttpPost("multiple")]
        public IActionResult PostMultiple([FromBody] Order[] orders)
        {
            StaticStorage.MarketOrders.AddRange(orders);
            return Ok(orders);
        }

        // GET: api/Orders/history
        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            return Ok(StaticStorage.TradeHistory);
        }

        // POST: api/Orders/history
        [HttpPost("history")]
        public IActionResult PostHistory([FromBody] Trade trade)
        {
            StaticStorage.TradeHistory.Add(trade);
            return Ok(trade);
        }
    }
}
