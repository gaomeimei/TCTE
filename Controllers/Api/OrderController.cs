using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
                    .Include(o => o.Client)
                    .Include(o => o.Terminal)
                    .Where(o => o.Terminal.AccessToken == token && o.Status != Models.SystemType.OrderStatus.Ended).Select(o => new
                    {
                        OrderId = o.Id,
                        CreateTime = o.CreatedDate,
                        ClientName = o.Client.Name,
                        ClientPhone = o.Client.Phone,
                        ClientAddress = o.Client.Address,
                        PlateNumber = o.Client.PlateNumber
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
        [Route("api/orders/{orderCode}")]
        public HttpResponseMessage Get(string orderCode)
        {
            return null;
        }
        [HttpPut]
        [Route("api/orders/{orderCode}")]
        public HttpResponseMessage Put([FromBody]string orderCode)
        {
            return null;
        }
    }
}
