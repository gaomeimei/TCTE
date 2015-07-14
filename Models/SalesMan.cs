using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCTE.Models.SystemType;
namespace TCTE.Models
{
    public class SalesMan
    {
        public int Id { get; set; }

        [Required, Display(Name = "姓名"), MaxLength(50)]
        public string Name { get; set; }
                
        [Required, Display(Name = "性别")]
        [UIHint("SystemTypeEnum")]
        public Gender Gender { get; set; }

        [Required, Display(Name = "身份证号"), MaxLength(21)]
        public string IdentityCard { get; set; }

        [Required]
        [MaxLength(50), RegularExpression(@"1\d{10}", ErrorMessage = "联系电话必须是11位手机号码")]
        [Display(Name = "联系电话")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "详细地址")]
        [MaxLength(200)]
        public string Address { get; set; }
        
        [Display(Name = "开始培训时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? TranningDate { get; set; }

        [Display(Name = "可否上岗")]
        public bool IsLicenced { get; set; }

        [Display(Name = "创建时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "备注")]
        public string Comment { get; set; }

        // 外键属性
        [Required]
        public int CompanyId { get; set; }
        public int? TerminalId { get; set; }

        // 导航属性
        public virtual Terminal Terminal { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        [Required]
        [Display(Name = "业务员编号"), MaxLength(50)]
        public string Code { get; set; }
    }
}
