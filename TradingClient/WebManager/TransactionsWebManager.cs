using IO.ApiClient.Api;
using IO.ApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingClient.Models;

namespace TradingClient.WebManager
{
    public static class TransactionsWebManager
    {
        public static string UserId { get; set; }

        public static async Task<List<Trade>> GetHistory()
        {
            var apiInstance = new OrdersApi();
            return await apiInstance.OrdersGetHistoryAsync();
        }

        public static async Task<List<Order>> GetOrders()
        {
            var apiInstance = new OrdersApi();
            return await apiInstance.OrdersGetAsync();
        }

        public static async Task<Order> PostOrder(Order order)
        {
            var apiInstance = new OrdersApi();
            return await apiInstance.OrdersPostAsync(order);
        }

        public static async Task<List<MarketOrderBook>> GetMarketOrders()
        {
            var apiInstance = new OrdersApi();
            var orders = await apiInstance.OrdersGetAsync();

            var marketGroups = orders.GroupBy(order => order.Price);

            var orderbooks = new List<MarketOrderBook>();

            foreach (var group in marketGroups)
            {
                var orderbook = new MarketOrderBook
                {
                    Price = group.Key.GetValueOrDefault(),
                    MyBidQty = group.Where(order => order.Side == SideEnum.Buy && order.UserId == UserId).Sum(order => order.Quantity).GetValueOrDefault(),
                    MktBidQty = group.Where(order => order.Side == SideEnum.Buy && order.UserId != UserId).Sum(order => order.Quantity).GetValueOrDefault(),
                    MktAskQty = group.Where(order => order.Side == SideEnum.Sell && order.UserId != UserId).Sum(order => order.Quantity).GetValueOrDefault(),
                    MyAskQty = group.Where(order => order.Side == SideEnum.Sell && order.UserId == UserId).Sum(order => order.Quantity).GetValueOrDefault()
                };
                orderbooks.Add(orderbook);
            }

            
            return orderbooks;
        }
    }
}
