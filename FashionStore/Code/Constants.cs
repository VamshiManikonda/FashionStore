using FashionStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace FashionStore.Code
{
    public static class Constants
    {
        public static class Application
        {
            public static User User
            {
                get
                {
                    HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (authCookie != null)
                    {
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        var serializer = new JavaScriptSerializer();
                        return (User)serializer.Deserialize(authTicket.UserData, typeof(User));
                    }
                    return null;
                }
            }

            public static OrderCart OrderCart
            {
                get
                {
                    if (HttpContext.Current.Session["OrderCart"] != null)
                    {
                        return (OrderCart)HttpContext.Current.Session["OrderCart"];
                    }
                    else
                    {
                        return new OrderCart();
                    }
                }
            }

            public static bool AdminUser
            {
                get
                {
                    if (User != null)
                        return User.AdminUser;
                    return false;
                }
            }

            public static string UserSession
            {
                get
                {
                    return (HttpContext.Current.Session["UserSession"] != null) ? HttpContext.Current.Session["UserSession"].ToString() : "";
                }
            }

            public static string DBConnConnectionString
            {
                get
                {
                    return ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                }
            }

            public static string BaseURL
            {
                get
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~");
                }
            }
        }
    }
}