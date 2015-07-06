using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCTE.Models.SystemType
{
    public enum Gender
    {
        [Display(Name = "男")]
        Male = 1,
        [Display(Name = "女")]
        Female = 2
    }
}
