using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionStore.Models
{
    public class Inventory
    {
        public Inventory()
        {
            InventoryProducts = new List<Product>();
        }
        public int ProductId { get; set; }
        public List<Product> InventoryProducts { get; set; }
    }
}