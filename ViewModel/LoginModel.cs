using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCTE.ViewModel
{
    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}