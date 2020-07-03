using IO.ApiClient.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TradingServer.Database_Connector;
using TradingServer.Services;

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
            ProcessOrderService.ProcessOrder(order);

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
