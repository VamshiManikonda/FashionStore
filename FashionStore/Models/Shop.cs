using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionStore.Models
{
    public class Shop
    {
        public Shop()
        {
            WomenProducts = new List<Product>();
            MenProducts = new List<Product>();
        }
        public int ProductId { get; set; }
        public List<Product> WomenProducts { get; set; }
        public List<Product> MenProducts { get; set; }
    }
}