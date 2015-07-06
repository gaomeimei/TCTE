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

        [Display(Name = "订单状态")]
        [UIHint("SystemTypeEnum")]
        public OrderStatus Status { get; set; }

        [Display(Name = "创建时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "开始处理时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "完成时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "备注"), MaxLength(200)]
        public string Comment { get; set; }

        // 外键属性
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int ClientId { get; set; }
        public int? SalesManId { get; set; }
        public int? TerminalId { get; set; }

        // 导航属性
        public virtual Company Company { get; set; }
        public virtual Client Client { get; set; }
        public virtual SalesMan SalesMan { get; set; }
        public virtual Terminal Terminal { get; set; }

        /// <summary>
        /// 订单编号规则: 商家编号 + 年 + 月 + 日 + 序列号
        /// </summary>
        [NotMapped, Display(Name = "订单编号")]
        public string Code { get; set; }
        
    }
}
