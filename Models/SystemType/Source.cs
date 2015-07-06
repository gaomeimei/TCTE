using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCTE.Models.SystemType
{
    public enum Source
    {
        [Display(Name = "电话")]
        Phone = 1,
        [Display(Name = "网站")]
        Website = 2,
        [Display(Name = "微信")]
        WebChat = 3
    }
}
