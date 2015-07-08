using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCTE.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required, MaxLength(20), Display(Name = "决定书编号")]
        public string DecisionNumber { get; set; }

        [Display(Name = "罚款金额")]
        public decimal Money { get; set; }

        [Display(Name = "是否交款")]
        public bool IsPay { get; set; }

        //外键属性
        [Required, Display(Name = "订单编号")]
        public int OrderId { get; set; }

        //导航属性
        public virtual Order Order { get; set; }
    }
}