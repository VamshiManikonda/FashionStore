using FashionStore.Code;
using FashionStore.Common;
using FashionStore.DAL;
using FashionStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionStore.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cart()
        {            
            var orderCarts = (List<OrderCart>)Session["OrderCartList"];
            var order = new OrderDAL().GetCart(orderCarts);
            return View("Cart", order);
        }

        public ActionResult Checkout()
        {
            if(Constants.Application.User == null)
            {
                Session["Checkout"] = true;
                string redirectTo = "~/Account/Login";
                return Redirect(redirectTo);
            }
            var orderCarts = (List<OrderCart>)Session["OrderCartList"];
            var order = new OrderDAL().GetCart(orderCarts);
            order.FirstName = Constants.Application.User.FirstName;
            order.LastName = Constants.Application.User.LastName;
            order.Email = Constants.Application.User.Email;
            order.Address = Constants.Application.User.Address;
            order.Phone = Constants.Application.User.Phone;
            order.City = Constants.Application.User.City;
            order.State = Constants.Application.User.State;
            order.Country = Constants.Application.User.Country;
            order.ZipCode = Constants.Application.User.ZipCode;
            return View("Checkout", order);
        }

        public ActionResult RemoveItem(int productId)
        {
            var orderCarts = (List<OrderCart>)Session["OrderCartList"];
            if (orderCarts.Count > 0 && orderCarts.Where(x => x.ProductId == productId).Count() > 0)
            {
                var orderCart = orderCarts.Where(x => x.ProductId == productId).FirstOrDefault();
                orderCarts.Remove(orderCart);
            }
            Session["OrderCartList"] = orderCarts;
            return RedirectToAction("Cart");
        }

        public ActionResult UpdateCart(string productName, string quantity)
        {
            productName = productName.Split(':')[1].Trim().Replace(")","");
            var orderCarts = (List<OrderCart>)Session["OrderCartList"];
            if (orderCarts.Count > 0 && orderCarts.Where(x => x.ProductId == Convert.ToInt32(productName)).Count() > 0)
            {
                var orderCart = orderCarts.Where(x => x.ProductId == Convert.ToInt32(productName)).FirstOrDefault();
                orderCarts.Remove(orderCart);
                orderCart.Quantity = Convert.ToInt32(quantity);
                orderCarts.Add(orderCart);
            }
            Session["OrderCartList"] = orderCarts;
            return RedirectToAction("Cart");
        }

        public ActionResult Confirm(Order model)
        {
            var orderCarts = (List<OrderCart>)Session["OrderCartList"];
            var res = new OrderDAL().GetCart(orderCarts);
            model.OrderCarts = res.OrderCarts;
            model.Quantity = res.OrderCarts.Sum(x => x.Quantity);
            model.SubTotal = res.OrderCarts.Sum(x => x.TotalPrice);
            model.Total = res.OrderCarts.Sum(x => x.TotalPrice);
            model.OrderStatus = "In process";
            var response = new OrderDAL().Confirm(model);
            new Notification().SendOrderEmail(model);
            Session["OrderCartList"] = null;
            return View("MyOrder", response);
        }

        public ActionResult CancelOrder(int orderId)
        {
            new OrderDAL().CancelOrder(orderId);
            var order = new OrderDAL().GetOrder(orderId);
            order.OrderId = orderId;
            order.OrderStatus = "Cancelled";
            new Notification().SendOrderStatusEmail(order);
            string redirectUrl = "~/Account/MyOrder";
            return Redirect(redirectUrl);
        }

        public ActionResult ProcessOrder(int orderId)
        {
            new OrderDAL().ProcessOrder(orderId);
            var order = new OrderDAL().GetOrder(orderId);
            order.OrderId = orderId;
            order.OrderStatus = "Processed";
            new Notification().SendOrderStatusEmail(order);
            string redirectUrl = "~/Account/OrderManager";
            return Redirect(redirectUrl);
        }

        public ActionResult GetOrder(int orderId)
        {
            var order = new OrderDAL().GetOrder(orderId);
            return PartialView("ViewOrder", order);
        }
    }
}