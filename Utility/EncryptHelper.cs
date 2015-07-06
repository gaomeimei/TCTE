using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCTE.Utility
{
    public static class EncryptHelper
    {
        public static string MD5Encrypt(string source)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5");
        }
    }
}