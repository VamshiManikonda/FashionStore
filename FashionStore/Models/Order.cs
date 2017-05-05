using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FashionStore.Models
{
    public class Order
    {
        public Order()
        {
            OrderCarts = new List<OrderCart>();
        }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }
        public string CreatedDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [Display(Name = "State / Province")]
        public string State { get; set; }
        public string Country { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Cash on delivery")]
        public bool Cash { get; set; }
        public bool Paypal { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public List<OrderCart> OrderCarts { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}