using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TCTE.Models
{
    public class Company
    {
        public int Id { get; set; } 

        [Required]
        [MaxLength(50)]
        [Display(Name = "商家名称")]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "商家简称")]
        public string Abbr { get; set; }

        [Display(Name = "首次服务时间"), DisplayFormat(DataFormatString="{0:yyyy-MM-dd}")]
        public Nullable<DateTime> FirstServiceDate { get; set; }

        [Display(Name = "服务次数")]
        public int OrderCount { get; set; }

        [Required]
        [Display(Name = "开设时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }

        [Required, Display(Name = "联系人"), MaxLength(50)]
        public string ContactName { get; set; }

        [Required]
        [MaxLength(50), RegularExpression(@"1\d{10}", ErrorMessage = "联系电话必须是11位手机号码")]
        [Display(Name = "联系电话")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "详细地址")]
        [MaxLength(200)]
        public string Address { get; set; }

        // 外键属性
        [Required, Display(Name = "城市")]
        public int CityId { get; set; }
        
        // 导航属性
        public virtual City City { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<SalesMan> SalesMen { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Terminal> Terminals { get; set; }

        [Display(Name = "商家编号"), MaxLength(50)]
        public string Code { get; set; }
    }
}
