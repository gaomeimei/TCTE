using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TCTE.Models.SystemType;
using System.ComponentModel.DataAnnotations.Schema;
namespace TCTE.Models
{

    public class Client
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "性别")]
        [UIHint("SystemTypeEnum")]
        public Gender Gender { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "车牌号")]
        public string PlateNumber { get; set; }

        [Required]
        [MaxLength(50), RegularExpression(@"1\d{10}", ErrorMessage = "联系电话必须是11位手机号码")]
        [Display(Name = "联系电话")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "详细地址")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "客户来源")]
        [UIHint("SystemTypeEnum")]
        public Source Source { get; set; }


        [Display(Name = "备注")]
        public string Comment { get; set; }

        // 外键属性
        [Required, Display(Name = "城市")]
        public int CityId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        // 导航属性
        public virtual City City { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        [Display(Name = "客户编号"),MaxLength(50)]
        public string Code { get; set; }
    }
}
