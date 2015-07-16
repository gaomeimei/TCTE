using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCTE.Models;
using TCTE.Models.SystemType;
using System.Configuration;

namespace TCTE.Utility
{
    public class SystemRole
    {
        public static string SUPER_ADMIN = "超级管理员";
        public static string COMPANY_ADMIN = "商家管理员";
    }
    public class RoleHelper
    {        
        /// <summary>
        /// 判断用户是否属于指定角色
        /// </summary>
        /// <param name="roleName"><see cref="TCTE.Utility.SystemRole"/></param>
        /// <returns></returns>
        public static bool IsInRole(string roleName)
        {
            var user = HttpContext.Current.Session["user"] as User;
            if (user == null) return false;
            if(string.IsNullOrEmpty(roleName)) return false;
            return roleName.ToLower() == user.Role.Name.ToLower();
        }
    }
}