using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionStore.Models
{
    public class OrderManager
    {
        public OrderManager()
        {
            CurrentOrders = new List<Order>();
            ProcessedOrders = new List<Order>();
        }
        public List<Order> CurrentOrders { get; set; }
        public List<Order> ProcessedOrders { get; set; }
    }
}