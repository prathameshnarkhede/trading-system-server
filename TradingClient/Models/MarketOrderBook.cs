using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingClient.Models
{
    public class MarketOrderBook
    {
        [DisplayName("My Bid Quantity")]
        public int MyBidQty { get; set; }

        [DisplayName("Market Bid Quantity")]
        public int MktBidQty { get; set; }

        [DisplayName("Price")]
        public double Price { get; set; }

        [DisplayName("Market Ask Quantity")]
        public int MktAskQty { get; set; }

        [DisplayName("My Ask Quantity")]
        public int MyAskQty { get; set; }
    }
}
