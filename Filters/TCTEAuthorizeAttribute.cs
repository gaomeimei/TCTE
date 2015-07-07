using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCTE.Utility;

namespace TCTE.Filters
{
    public class SuperAdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return RoleHelper.IsInRole(SystemRole.SUPER_ADMIN);
        }
    }
    public class CompanyAdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return RoleHelper.IsInRole(SystemRole.COMPANY_ADMIN);
        }
    }
}