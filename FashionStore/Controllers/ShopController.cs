using FashionStore.DAL;
using FashionStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionStore.Controllers
{
    public class ShopController : BaseController
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Women()
        {
            var model = new Shop();
            model.WomenProducts = new ShopDAL().WomenProducts();
            return View("Women", model);
        }

        public ActionResult Men()
        {
            var model = new Shop();
            model.MenProducts = new ShopDAL().MenProducts();
            return View("Men", model);
        }

        public ActionResult Kid()
        {
            return View();
        }
        
        public ActionResult Product(int productId)
        {
            var product = new ShopDAL().Product(productId);
            return View("Product", product);
        }

        public ActionResult AddToCart(int productId)
        {
            var orderCartList = new List<OrderCart>();
            if (Session["OrderCartList"] == null)
            {
                var orderCart = new OrderCart
                {
                    ProductId = productId,
                    Quantity = 1,
                };
                orderCartList.Add(orderCart);                
            }
            else
            {
                orderCartList = (List<OrderCart>) Session["OrderCartList"];
                if(orderCartList.Count > 0 && orderCartList.Where(x => x.ProductId == productId).Count() > 0)
                {
                    var orderCart = orderCartList.Where(x => x.ProductId == productId).FirstOrDefault();
                    orderCartList.Remove(orderCart);
                    orderCart.Quantity = orderCart.Quantity + 1;
                    orderCartList.Add(orderCart);
                }
                else
                {
                    var orderCart = new OrderCart
                    {
                        ProductId = productId,
                        Quantity = 1,
                    };
                    orderCartList.Add(orderCart);
                }
            }
            Session["OrderCartList"] = orderCartList;
            return Redirect(Request.UrlReferrer.ToString());

        }
    }
}