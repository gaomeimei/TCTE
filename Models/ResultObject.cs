using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCTE.Models
{
    public class APIResultObject
    {
        public static readonly int UnAuthorized = 101;
        public static readonly int WaittingApproved = 102;
        public static readonly int InValidRequest = 103;
        public static readonly int BadRequest = 104;
        public static readonly int InValidStatus = 105;
        public static readonly int OK = 200;
        public static readonly int ServerError = 500;
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public object Result { get; set; }
    }
}