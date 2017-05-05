using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionStore.Models
{
    public class HomeDashboard
    {
        public HomeDashboard()
        {
            TopWomenProducts = new List<Product>();
            TopMenProducts = new List<Product>();
        }
        public int ProductId { get; set; }
        public List<Product> TopWomenProducts { get; set; }
        public List<Product> TopMenProducts { get; set; }
    }
}