using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCTE.Models.SystemType;

namespace TCTE.Models
{
    public class Terminal
    {
        public int Id { get; set; }

        [Required, Display(Name = "设备状态")]
        [UIHint("SystemTypeEnum")]
        public TerminalStatus Status { get; set; }
        //[Required,MaxLength(50)]
        public string AccessToken { get; set; }
        public string FingerPrint { get; set; }
        public Nullable<DateTime> LastInitialDate { get; set; }
        public DateTime CreateDate { get; set; }

        // 外键属性
        [Display(Name = "授权商家")]
        public int? CompanyId { get; set; }
        [Display(Name = "业务员")]
        public int? SalesManId { get; set; }

        // 导航属性
        public virtual Company Company { get; set; }
        public virtual SalesMan SalesMan { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        [Display(Name = "设备编号"),MaxLength(50)]
        public string Code { get; set; }
    }
}
