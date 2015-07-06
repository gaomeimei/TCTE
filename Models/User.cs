using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace TCTE.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "创建日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "最后登陆日期"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> LastLoginDate { get; set; }

        [Display(Name = "额外查询次数")]
        public int ExtraQueryCount { get; set; }

        // 外键属性
        public int? CompanyId { get; set; }
        public int? RoleId { get; set; }

        // 导航属性
        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }

        
    }
}
