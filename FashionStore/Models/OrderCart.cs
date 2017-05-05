using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionStore.Models
{
    public class OrderCart
    {
        public OrderCart()
        {
            Quantities = new List<SelectListItem>();
        }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public string ItemQuantity { get; set; }
        public int OrderId { get; set; }
        public IEnumerable<SelectListItem> Quantities { get; set; }
    }
}