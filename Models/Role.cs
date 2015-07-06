using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCTE.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required, MaxLength(50), Display(Name = "角色名称")]
        public string Name { get; set; }

        [Display(Name = "角色描述")]
        public string Description { get; set; }

        // 导航属性
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Function> Functions { get; set; }
    }
}