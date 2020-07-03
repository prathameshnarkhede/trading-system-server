using IO.ApiClient.Model;
using System.Collections.Generic;

namespace TradingServer.Models
{
    public class Tick
    {
        public List<Order> Orders { get; set; }
    }
}
