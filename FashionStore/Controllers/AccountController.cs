using FashionStore.Code;
using FashionStore.Common;
using FashionStore.DAL;
using FashionStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace FashionStore.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Login()
        {
            return View("Login", new Login());
        }

        public ActionResult LoginUser(Login model)
        {
            var response = new CustomerDAL().Login(model);
            if (response.Status > 0)
            {
                ViewBag.ErrorMessage = model.Message;
                return View("Login", response);
            }
            else
            {
                Session["UserSession"] = model.User.UserName;
                //FormsAuthentication.SetAuthCookie(model.UserName, false);
                var serializer = new JavaScriptSerializer();
                string userData = serializer.Serialize(model.User);

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        model.UserName,
                        DateTime.Now,
                        DateTime.Now.AddHours(8),
                        true,
                        userData,
                        FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                if(Session["Checkout"] != null && (Boolean)Session["Checkout"])
                {
                    Session["Checkout"] = false;
                    string redirectTo = "~/Order/Checkout";
                    return Redirect(redirectTo);
                }
                return RedirectToAction("MyOrder");
            }
        }

        public ActionResult Dashboard()
        {
            return View("Dashboard", Constants.Application.User);
        }

        public ActionResult MyOrder()
        {
            var order = new OrderDAL().MyOrders();
            return View("MyOrder", order);
        }

        public ActionResult OrderManager()
        {
            var order = new OrderDAL().GetOrders();
            return View("OrderManager", order);
        }

        public ActionResult Register()
        {
            return View("Register", new Login());
        }

        public ActionResult Registration(Login model)
        {
            var response = new CustomerDAL().Register(model);
            if(response.Status > 0)
            {
                ViewBag.ErrorMessage = model.Message;
                return View("Register", response);
            }
            else
            {
                new Notification().SendWelcomeEmail(response);
                return RedirectToAction("Login");
            }
        }

        public ActionResult Reset()
        {            
            if(Constants.Application.User != null && !string.IsNullOrEmpty(Constants.Application.User.UserName))
            {
                var login = new Login();
                login.UserName = Constants.Application.User.UserName;
                return View("Reset", login);
            }
            return View();
        }

        public ActionResult ResetPassword(Login model)
        {
            var response = new CustomerDAL().Reset(model);
            if (response.Status > 0)
            {
                ViewBag.ErrorMessage = model.Message;
                return View("Reset", response);
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }

        public ActionResult Logout()
        {
            //Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Inventory()
        {
            Inventory inventory = new Inventory();
            inventory.InventoryProducts = new InventoryDAL().GetProducts();
            return View("Inventory", inventory);
        }

        public ActionResult AddProduct()
        {
            return PartialView("AddEditProduct", new Product());
        }

        public ActionResult GetProduct(int productId)
        {
            var product = new ShopDAL().Product(productId);
            return PartialView("AddEditProduct", product);
        }

        public ActionResult RemoveProduct(int productId)
        {
            new InventoryDAL().RemoveProduct(productId);
            return RedirectToAction("Inventory");
        }

        public ActionResult SaveProduct(Product model)
        {
            new InventoryDAL().SaveProduct(model);
            return RedirectToAction("Inventory");
        }

        public ActionResult UpdateAccount(User model)
        {
            model = new CustomerDAL().Update(model);
            if (model.Status > 0)
            {
                ViewBag.ErrorMessage = model.Message;
                return View("Dashboard", model);
            }
            else
            {
                FormsAuthentication.SignOut();
                Session["UserSession"] = model.UserName;
                var serializer = new JavaScriptSerializer();
                string userData = serializer.Serialize(model);

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        model.UserName,
                        DateTime.Now,
                        DateTime.Now.AddHours(8),
                        true,
                        userData,
                        FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                return RedirectToAction("Dashboard");
            }
        }

        public JsonResult UploadFile()
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        var fileName = Path.GetFileName(fileContent.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        return Json(fileName, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

            return Json("false", JsonRequestBehavior.AllowGet);
        }
    }
}