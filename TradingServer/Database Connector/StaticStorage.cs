using IO.ApiClient.Model;
using System.Collections.Generic;

namespace TradingServer.Database_Connector
{
    public static class StaticStorage
    {
        public static List<Order> MarketOrders { get; set; } = new List<Order>();


        public static List<Trade> TradeHistory { get; set; } = new List<Trade>();

    }
}
