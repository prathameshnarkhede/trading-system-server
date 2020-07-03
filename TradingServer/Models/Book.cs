using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingServer.Models
{
    public class Book
    {
        public double Price { get; set; }

        public int MarketQuantity { get; set; }

        public int AskQuantity { get; set; }
    }
}
