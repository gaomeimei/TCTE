using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TCTE.Models;
using TCTE.Filters;
using TCTE.ViewModel;
using System.Threading;
namespace TCTE.Controllers.Api
{
    [IdentityBasicAuthentication(true)]
    public class TerminalController : ApiController
    {
        /// <summary>
        /// Register terminal
        /// </summary>
        /// <returns></returns>
        [IdentityBasicAuthentication(false)]
        [HttpGet]
        [Route("api/Terminal/Register/{token}")]
        public HttpResponseMessage Register(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                using (var context = new TCTEContext())
                {
                    var tokenEnity = context.RegistrationTokens.Where(r => r.Token == token).SingleOrDefault();
                    if (tokenEnity != null)
                    {
                        string refreshToken = Guid.NewGuid().ToString();
                        context.RegistrationRequests.Add(new RegistrationRequest()
                        {
                            RequestDate = DateTime.Now,
                            Status = Models.SystemType.RegistrationRequestStatus.WaitingApprove,
                            RegistrationTokenId = tokenEnity.Id,
                            RefreshToken = refreshToken
                        });
                        context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.OK,
                            Description = "success",
                            Result = refreshToken
                        });
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, new APIResultObject()
            {
                StatusCode = APIResultObject.UnAuthorized,
                Description = "请提供正确的授权代码",
                Result = ""
            });
        }
        /// <summary>
        /// Device send the request activate request
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [IdentityBasicAuthentication(false)]
        [HttpGet]
        [Route("api/Terminal/Activate/{refreshToken}")]
        public HttpResponseMessage Activate(string refreshToken)
        {
            if (!string.IsNullOrEmpty(refreshToken))
            {
                using (var context = new TCTEContext())
                {
                    var request = context.RegistrationRequests.Where(r => r.RefreshToken == refreshToken).SingleOrDefault();
                    if (request != null)
                    {
                        if (request.Status == Models.SystemType.RegistrationRequestStatus.Approved)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                            {
                                StatusCode = APIResultObject.OK,
                                Description = "success",
                                Result = request.AccessToken
                            });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                            {
                                StatusCode = APIResultObject.WaittingApproved,
                                Description = "success",
                                Result = ""
                            });
                        }
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, new APIResultObject()
            {
                StatusCode = APIResultObject.UnAuthorized,
                Description = "请提供正确的授权代码",
                Result = ""
            });
        }
        /// <summary>
        /// Device init request
        /// </summary>
        /// <returns></returns>
        [IdentityBasicAuthentication(false)]
        [HttpPost]
        [Route("api/Terminal/Init")]
        public HttpResponseMessage Init([FromBodyAttribute] TerminalInitViewModel model)
        {
            if (ModelState.IsValid)
            {
                string accessToken = model.AccessToken;
                using (var context = new TCTEContext())
                {
                    var ternimal = context.Terminals.Where(t => t.Status == Models.SystemType.TerminalStatus.NotInitialized && t.AccessToken == accessToken).SingleOrDefault();

                    if (ternimal != null)
                    {
                        var salesMan = context.SalesMen.Where(s => s.Code == model.SalesManCode && s.CompanyId == ternimal.CompanyId && !s.TerminalId.HasValue && s.IsLicenced).SingleOrDefault();
                        if (salesMan != null)
                        {
                            salesMan.TerminalId = ternimal.Id;
                            ternimal.SalesManId = salesMan.Id;
                            ternimal.FingerPrint = model.FingerPrint;
                            ternimal.LastInitialDate = DateTime.Now;
                            ternimal.Status = Models.SystemType.TerminalStatus.Normal;
                            context.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                            {
                                StatusCode = APIResultObject.OK,
                                Description = "success",
                                Result = ""
                            });
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new APIResultObject()
                       {
                           StatusCode = APIResultObject.BadRequest,
                           Description = "设备或者业务人员不存在或处于绑定状态",
                           Result = ""
                       });
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, new APIResultObject()
            {
                StatusCode = APIResultObject.InValidRequest,
                Description = "请提供正确参数格式，以及完整参数",
                Result = ""
            });
        }
        /// <summary>
        /// Device status
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [IdentityBasicAuthentication(false)]
        [HttpGet]
        [Route("api/Terminal/Status/{accessToken}")]
        public HttpResponseMessage GetStatus(string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                using (var context = new TCTEContext())
                {
                    var ternimal = context.Terminals.Where(t => t.AccessToken == accessToken).SingleOrDefault();
                    return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                    {
                        StatusCode = APIResultObject.OK,
                        Description = "success",
                        Result = ternimal.Status.ToString()
                    });
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, new APIResultObject()
            {
                StatusCode = APIResultObject.InValidRequest,
                Description = "请提供正确参数格式，以及完整参数",
                Result = ""
            });
        }
        [HttpPost]
        [Route("api/Terminal/Verify")]
        public HttpResponseMessage Verify([FromBody] string fingerPrint)
        {
            string token = GetToken();
            using (var context = new TCTEContext())
            {
                var ternimal = context.Terminals.Where(t => t.AccessToken == token && (t.FingerPrint == fingerPrint)).SingleOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                {
                    StatusCode = APIResultObject.OK,
                    Description = "success",
                    Result = ternimal==null? false: true 
                });
            }
        }
        private string GetToken()
        {
            return (Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity).AccessToken;
        }

    }
}
