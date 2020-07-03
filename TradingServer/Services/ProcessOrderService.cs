using IO.ApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingServer.Database_Connector;

namespace TradingServer.Services
{
    public class ProcessOrderService
    {
        public static void ProcessOrder(Order order)
        {
            // For Buy order
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
                }

            }

            // For Sell order
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
                }
            }
        }
    }
}
