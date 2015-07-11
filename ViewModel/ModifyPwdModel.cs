using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCTE.ViewModel
{
    public class ModifyPwdModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "原密码")]
        public string OldPwd { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "新密码")]
        public string NewPwd { get; set; }

        [Required]
        [MaxLength(50)]
        [Compare("NewPwd")]
        [Display(Name = "确认新密码")]
        public string NewPwdConfirm { get; set; }
    }
}