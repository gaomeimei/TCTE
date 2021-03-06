﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using TCTE.ViewModel;
using TCTE.Filters;
using TCTE.Models;
namespace TCTE.Controllers.Api
{
    [IdentityBasicAuthentication(true)]
    public class OrderController : ApiController
    {
        [HttpGet]
        [Route("api/orders")]
        public HttpResponseMessage Get()
        {
            //get token
            string token = (System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity).AccessToken;
            //get orders
            using (var context = new TCTEContext())
            {
                var orders = context.Orders
                    .Where(o => o.Terminal.AccessToken == token && o.Status != Models.SystemType.OrderStatus.Ended).Select(o => new
                    {
                        OrderId = o.Id,
                        OrderCode = o.Code,
                        CreateTime = o.CreatedDate,
                        ClientName = o.Name,
                        ClientPhone = o.Phone,
                        ClientAddress = o.Address,
                        PlateNumber = o.PlateNumber,
                        State = o.Status,
                        Comment = o.Comment
                    }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, new APIResultObject()
                {
                    StatusCode = APIResultObject.OK,
                    Description = "success",
                    Result = orders
                });
            }
        }
        [HttpGet]
        [Route("api/orders/Start/{orderCode}")]
        public HttpResponseMessage Start([FromUri]string orderCode)
        {
            string token = GetToken();
            using (var db = new TCTEContext())
            {
                var order = db.Orders.Where(o => o.Code == orderCode && o.Terminal.AccessToken == token).SingleOrDefault();
                if (order != null)
                {
                    order.Status = Models.SystemType.OrderStatus.Started;
                    order.StartTime = DateTime.Now;
                    db.SaveChanges();
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
                StatusCode = APIResultObject.NotFound,
                Description = "success",
                Result = "没有找到请求的订单编号"
            });
        }
        [HttpGet]
        [Route("api/orders/Complete/{orderCode}")]
        public HttpResponseMessage Complete([FromUri] string orderCode)
        {
            string token = GetToken();
            using (var db = new TCTEContext())
            {
                var order = db.Orders.Where(o => o.Code == orderCode && o.Terminal.AccessToken == token).SingleOrDefault();
                if (order != null)
                {
                    order.Status = Models.SystemType.OrderStatus.Ended;
                    order.EndTime = DateTime.Now;
                    db.SaveChanges();
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
                StatusCode = APIResultObject.NotFound,
                Description = "没有找到请求的订单编号",
                Result = ""
            });
        }

        [HttpPost]
        [Route("api/orders/punish")]
        public HttpResponseMessage Punish([FromBody] Punishment punish)
        {
            string token = GetToken();
            using (var db = new TCTEContext())
            {
                var order = db.Orders.Where(o => o.Code.ToLower() == punish.OrderCode.ToLower() && o.Terminal.AccessToken == token).SingleOrDefault();
                if (order != null)
                {

                    order.OrderDetails.Add(new OrderDetail()
                    {
                        DecisionNumber = punish.DecisionNumber,
                        Deduction = punish.Dedution,
                        PeccancyAddress = punish.PeccancyAddress,
                        PeccancyBehavior  = punish.PeccancyBehavior,
                        PeccancyTime = punish.PeccancyTime,
                        Money = punish.Money,
                        PeccancyPersonNo = punish.PeccancyPersonNo,
                        HandlePersonNo = punish.HandlePersonNo
                    });
                    if (punish.Images != null && punish.Images.Length > 0)
                    {
                        foreach (var image in punish.Images)
                        {
                            db.OrderImages.Add(new OrderImage()
                            {
                                DecisionNumber = punish.DecisionNumber,
                                ImageContent = image
                            });
                        }
                    }
                    db.SaveChanges();
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
                StatusCode = APIResultObject.NotFound,
                Description = "没有找到请求的订单编号",
                Result = ""
            });

        }

        [HttpPost]
        [Route("api/orders/pay")]
        public HttpResponseMessage Pay(string decisionNumber, string bankTransactionNumber)
        {
            string token = GetToken();
            using (var db = new TCTEContext())
            {
                var detail = db.OrderDetails.Where(o => o.DecisionNumber == decisionNumber).SingleOrDefault();
                if (detail != null)
                {
                    detail.IsPay = true;
                    detail.BankSequenceNumber = bankTransactionNumber;
                    db.SaveChanges();
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
                StatusCode = APIResultObject.NotFound,
                Description = "没有找到请求的决定书编号",
                Result = ""
            });
        }

        [IdentityBasicAuthentication(false)]
        [HttpGet]
        [Route("api/decision/{decisionNumber}/{isPay}")]
        public HttpResponseMessage Pay(string decisionNumber,int isPay)
        {
            using (var db = new TCTEContext())
            {
                var detail = db.OrderDetails.Where(o => o.DecisionNumber == decisionNumber).SingleOrDefault();
                if (detail != null)
                {
                    detail.IsPay = isPay==0?false:true;
                    db.SaveChanges();
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
                StatusCode = APIResultObject.NotFound,
                Description = "没有找到请求的决定书编号",
                Result = ""
            });
        }

        private string GetToken()
        {
            return (Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity).AccessToken;
        }
    }
}
