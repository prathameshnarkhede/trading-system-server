using IO.ApiClient.Model;
using System.Collections.Generic;

namespace TradingServer.Models
{
    public class User
    {
        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }
}
