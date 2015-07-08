using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCTE.ViewModel
{
    /// <summary>
    /// 车辆违法信息
    /// </summary>
    public class PeccancyInfo
    {
        [Display(Name = "车牌号码")]
        public string PlateNumber { get; set; }

        [Display(Name = "违法时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}")]
        public DateTime Time { get; set; }

        [Display(Name = "违法地点")]
        public string Address { get; set; }

        [Display(Name = "违法行为")]
        public string Behavior { get; set; }

        [Display(Name = "罚款金额")]
        public decimal Money { get; set; }

        [Display(Name = "违法扣分")]
        public int Deduction { get; set; }
    }
}