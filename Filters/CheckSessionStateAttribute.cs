using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCTE.Filters
{
    /// <summary>
    /// 检查Session状态, 如果Session已过期, 重定向到~/Home/Login
    /// </summary>
    public class CheckSessionStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.Session["user"] == null)
            {
                var ReturnUrl = filterContext.HttpContext.Request.Url.ToString();
                filterContext.HttpContext.Response.Redirect("~/Home/Login?ReturnUrl=" + HttpUtility.UrlEncode(ReturnUrl));
            }
        }
    }
}