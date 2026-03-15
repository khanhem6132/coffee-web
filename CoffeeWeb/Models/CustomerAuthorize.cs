using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeWeb.Models
{
    public class CustomerAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}