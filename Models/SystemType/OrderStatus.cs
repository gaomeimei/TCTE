using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCTE.Models.SystemType
{
    public enum OrderStatus
    {
        [Display(Name = "已创建")]
        Created = 1,
        [Display(Name = "正在处理")]
        Started = 2, 
        [Display(Name = "已完成")]
        Ended = 3
    }
}