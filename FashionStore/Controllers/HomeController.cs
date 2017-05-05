using FashionStore.DAL;
using FashionStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionStore.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            var home = new HomeDashboard();
            home.TopMenProducts = new ShopDAL().MenProducts();
            home.TopWomenProducts = new ShopDAL().WomenProducts();
            return View("Index", home);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}