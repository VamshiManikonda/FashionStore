using FashionStore.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Required")]
        public decimal Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        [Display(Name = "Product Type")]
        [Required(ErrorMessage = "Required")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Required")]
        public bool Active { get; set; }
        public IEnumerable<SelectListItem> Types
        {
            get
            {
                var aps = new List<SelectListItem>();
                foreach (string value in Enum.GetNames(typeof(ProductType)))
                {
                    aps.Add(new SelectListItem
                    {
                        Value = value,
                        Text = ((ProductType)Enum.Parse(typeof(ProductType), value)).GetDescription(),
                    });
                }
                return aps;
            }
        }
    }
}