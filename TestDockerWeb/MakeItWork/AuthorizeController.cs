using TestDockerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace TestDockerWeb.MakeItWork
{
    public class AdminFilterController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            Account acc = HttpContext.Current.Session["User"] as Account;

            if (acc.isadmin==false)
            {
                filterContext.Result = new RedirectResult("~/Home/UserContainer");
            }
        }
    }

    public class LoginController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            Account acc = HttpContext.Current.Session["User"] as Account;

            if (acc == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Login");
            }
        }
    }
}