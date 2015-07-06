using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TCTE.Models.SystemType;
namespace TCTE.Models
{
    public class RegistrationRequest
    {
        public int Id { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        [Required, Display(Name = "请求状态")]
        public RegistrationRequestStatus Status { get; set; }
        [Required, Display(Name = "请求日期")]
        public DateTime RequestDate { get; set; }

        public Nullable<DateTime> ApproveDate { get; set; }
        public string AccessToken { get; set; }

        //外键属性
        [Required, Display(Name = "请求代码")]
        public int RegistrationTokenId { get; set; }

        //导航属性        
        public RegistrationToken RegistrationToken { get; set; }
    }
}