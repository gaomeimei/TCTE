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
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "车牌号")]
        public string PlateNumber { get; set; }

        [Required, MaxLength(50), Display(Name = "车架号")]
        public string VIN { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50), RegularExpression(@"1\d{10}", ErrorMessage = "联系电话必须是11位手机号码")]
        [Display(Name = "联系电话")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "详细地址")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Display(Name = "订单状态")]
        [UIHint("SystemTypeEnum")]
        public OrderStatus Status { get; set; }

        [Display(Name = "创建时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "开始处理时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "完成时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "备注"), MaxLength(200)]
        public string Comment { get; set; }

        // 外键属性
        [Required]
        public int CompanyId { get; set; }
        public int? ClientId { get; set; }
        [Display(Name = "业务员")]
        public int SalesManId { get; set; }
        public int? TerminalId { get; set; }

        // 导航属性
        public virtual Company Company { get; set; }
        public virtual Client Client { get; set; }
        public virtual SalesMan SalesMan { get; set; }
        public virtual Terminal Terminal { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        /// <summary>
        /// 订单编号规则: 商家编号 + 年 + 月 + 日 + 序列号
        /// </summary>
        [Display(Name = "订单编号")]
        public string Code { get; set; }
        
    }
}
