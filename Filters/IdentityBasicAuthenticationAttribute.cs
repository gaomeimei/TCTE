using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Security.Principal;
using System.Threading;
using System.Text;
using System.Net;
using TCTE.Models;
namespace TCTE.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class IdentityBasicAuthenticationAttribute : AuthorizationFilterAttribute
    {

        bool Active = true;

        public IdentityBasicAuthenticationAttribute()
        { }

        /// <summary>  
        /// Overriden constructor to allow explicit disabling of this  
        /// filter's behavior. Pass false to disable (same as no filter  
        /// but declarative)  
        /// </summary>  
        /// <param name="active"></param>  
        public IdentityBasicAuthenticationAttribute(bool active)
        {
            Active = active;
        }


        /// <summary>  
        /// Override to Web API filter method to handle Basic Auth check  
        /// </summary>  
        /// <param name="actionContext"></param>  
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Active)
            {
                var identity = ParseAuthorizationHeader(actionContext);
                if (identity == null)
                {
                    return;
                }

                var principal = new GenericPrincipal(identity, null);

                Thread.CurrentPrincipal = principal;

                base.OnAuthorization(actionContext);
            }
        }

        /// <summary>  
        /// Base implementation for user authentication - you probably will  
        /// want to override this method for application specific logic.  
        ///   
        /// The base implementation merely checks for username and password  
        /// present and set the Thread principal.  
        ///   
        /// Override this method if you want to customize Authentication  
        /// and store user data as needed in a Thread Principle or other  
        /// Request specific storage.  
        /// </summary>  
        /// <param name="username"></param>  
        /// <param name="password"></param>  
        /// <param name="actionContext"></param>  
        /// <returns></returns>  
        protected virtual bool OnAuthorizeUser(string apiKey, HttpActionContext actionContext)
        {
            if (apiKey == null)
                return false;

            return true;
        }

        /// <summary>  
        /// Parses the Authorization header and creates user credentials  
        /// </summary>  
        /// <param name="actionContext"></param>  
        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            //get api key from authorization header
            if (auth != null && auth.Scheme == "Basic")
            {
                authHeader = auth.Parameter;
            }
            //get api key from query string
            if (string.IsNullOrEmpty(authHeader) && actionContext.Request.GetQueryNameValuePairs().Where(k => k.Key.ToUpper() == "TOKEN").Count() > 0)
            {
                authHeader = "token=" + actionContext.Request.GetQueryNameValuePairs().Where(k => k.Key.ToUpper() == "TOKEN").Select(k => k.Value).SingleOrDefault();
            }
            string token = "";
            //check header format
            if (!string.IsNullOrEmpty(authHeader) && authHeader.ToUpper().Contains("TOKEN="))
            {
                token = authHeader.Split('=')[1];
                using (var context = new TCTEContext())
                {
                    var ternimal = context.Terminals.Where(t => t.AccessToken == token).SingleOrDefault();
                    if (ternimal != null)
                    {
                        if (ternimal.Status == Models.SystemType.TerminalStatus.Normal)
                        {
                            return new BasicAuthenticationIdentity(token);
                        }
                        else
                        {
                            //invalid status
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK,
                                 new APIResultObject
                                 {
                                     StatusCode = APIResultObject.InValidStatus,
                                     Description = "设备状态异常，请求无效！",
                                     Result = ternimal.Status
                                 }
                             );
                        }
                    }

                }
            }
            else
            {
                //no token
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK,
                    new APIResultObject
                    {
                        StatusCode = APIResultObject.UnAuthorized,
                        Description = "请提供授权代码！",
                        Result = null
                    }
                );
            }
            return null;
        }
    }
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string accessToken)
            : base("API", "Basic")
        {
            this.AccessToken = accessToken;
        }

        /// <summary>  
        /// Basic Auth Password for custom authentication  
        /// </summary>  
        public string AccessToken { get; set; }
    }  
}