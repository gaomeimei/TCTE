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
            return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
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
            return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
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
                    var terminal = context.Terminals.Where(t =>t.AccessToken == accessToken).SingleOrDefault();
                    if (terminal == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.InvalidToken,
                            Description = "授权码错误",
                            Result = ""
                        });
                    }
                    if (terminal.Status != Models.SystemType.TerminalStatus.NotInitialized)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.InValidStatus,
                            Description = "设备状态异常，不能进行员工绑定",
                            Result = ""
                        });
                    }
                    var salesMan = context.SalesMen.Where(s => s.Code.ToLower() == model.SalesManCode.ToLower() && s.CompanyId == terminal.CompanyId).SingleOrDefault();
                    if (salesMan == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.InvalidCode,
                            Description = "员工编码错误",
                            Result = ""
                        });
                    }
                    if (salesMan.TerminalId > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.InvalidBinding,
                            Description = "员工已经绑定终端",
                            Result = ""
                        });
                    }
                    salesMan.TerminalId = terminal.Id;
                    terminal.SalesManId = salesMan.Id;
                    terminal.FingerPrint = model.FingerPrint;
                    terminal.LastInitialDate = DateTime.Now;
                    terminal.Status = Models.SystemType.TerminalStatus.Normal;
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                    {
                        StatusCode = APIResultObject.OK,
                        Description = "success",
                        Result = ""
                    });
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
            {
                StatusCode = APIResultObject.InValidRequest,
                Description = "请提供完整参数",
                Result = ""
            });
        }

        /// <summary>
        /// Device init request
        /// </summary>
        /// <returns></returns>
        [IdentityBasicAuthentication(false)]
        [HttpPost]
        [Route("api/Terminal/Init_V2")]
        public HttpResponseMessage Init_V2([FromBodyAttribute] TerminalInitViewModel_V2 model)
        {
            if (ModelState.IsValid)
            {
                string accessToken = model.AccessToken;
                using (var context = new TCTEContext())
                {
                    var terminal = context.Terminals.Where(t => t.AccessToken == accessToken).SingleOrDefault();
                    if (terminal == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.InvalidToken,
                            Description = "授权码错误",
                            Result = ""
                        });
                    }
                    if (terminal.Status != Models.SystemType.TerminalStatus.NotInitialized)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.InValidStatus,
                            Description = "设备状态异常，不能进行员工绑定",
                            Result = ""
                        });
                    }
                    var salesMan = context.SalesMen.Where(s => s.Code.ToLower() == model.SalesManCode.ToLower() && s.CompanyId == terminal.CompanyId).SingleOrDefault();
                    if (salesMan == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.InvalidCode,
                            Description = "员工编码错误",
                            Result = ""
                        });
                    }
                    if (salesMan.IdentityCard != model.PersonCardNo)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.InvalidPersonCode,
                            Description = "身份证号码不匹配",
                            Result = ""
                        });
                    }
                    if (salesMan.TerminalId > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                        {
                            StatusCode = APIResultObject.InvalidBinding,
                            Description = "员工已经绑定终端",
                            Result = ""
                        });
                    }
                    salesMan.TerminalId = terminal.Id;
                    terminal.SalesManId = salesMan.Id;
                    //terminal.FingerPrint = model.FingerPrint;
                    terminal.LastInitialDate = DateTime.Now;
                    terminal.Status = Models.SystemType.TerminalStatus.Normal;
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                    {
                        StatusCode = APIResultObject.OK,
                        Description = "success",
                        Result = ""
                    });
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
            {
                StatusCode = APIResultObject.InValidRequest,
                Description = "请提供完整参数",
                Result = ""
            });
        }
        /// <summary>
        /// Device status
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Terminal/Status")]
        public HttpResponseMessage GetStatus()
        {
            string accessToken = GetToken();
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
                    Description = ternimal==null?"failed":"success",
                    Result = ternimal==null? false: true 
                });
            }
        }
        [HttpPost]
        [Route("api/Terminal/Verify_V2")]
        public HttpResponseMessage Verify_V2([FromBody] string personNo)
        {
            string token = GetToken();
            using (var context = new TCTEContext())
            {
                var ternimal = context.Terminals.Where(t => t.AccessToken == token && t.SalesMan.IdentityCard == personNo).SingleOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                {
                    StatusCode = APIResultObject.OK,
                    Description = ternimal == null ? "failed" : "success",
                    Result = ternimal == null ? false : true
                });
            }
        }
        private string GetToken()
        {
            return (Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity).AccessToken;
        }

    }
}
