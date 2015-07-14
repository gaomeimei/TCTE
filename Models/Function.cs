using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCTE.Models
{
    public class Function
    {
        public int Id { get; set; }

        [Required, Display(Name = "功能名称"), MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "功能描述"), MaxLength(200)]
        public string Description { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        // 导航属性
        public virtual ICollection<Role> Roles { get; set; }
    }
}