using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FashionStore.App_Start
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckSessionOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
           if (!HttpContext.Current.Response.IsClientConnected || authCookie == null)
           {
               //HttpContext.Current.Session.Abandon();
               //FormsAuthentication.SignOut();
               string redirectTo = "~/Account/SessionExpire";
               filterContext.Result = new RedirectResult(redirectTo);
               return;
           }
            base.OnActionExecuting(filterContext);
        }
    }
}