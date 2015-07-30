using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using TCTE.Models;
namespace TCTE.Filters
{
    public class APIExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK,
                   new APIResultObject()
                   {
                       StatusCode = APIResultObject.ServerError,
                       Description = actionExecutedContext.Exception.Message,
                       Result = ""
                   }
               );
        }
    }
}