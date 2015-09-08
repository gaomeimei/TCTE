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
    /// <summary>
    /// 车辆信息
    /// </summary>
    public class Car
    {
        [Display(Name = "号牌种类")]
        public string Type { get; set; }
        [Display(Name = "车牌号码")]
        public string PlatNumber { get; set; }
        [Display(Name = "使用性质")]
        public string Purpose { get; set; }
        [Display(Name = "机动车所有人")]
        public string Owner { get; set; }
        [Display(Name = "检验有效期止"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}")]
        public DateTime EndDate1 { get; set; }
        [Display(Name = "强制报废期止"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}")]
        public DateTime EndDate2 { get; set; }
        [Display(Name = "所有人手机号码")]
        public string PhoneNumber { get; set; }
        [Display(Name = "机动车状态")]
        public string Status { get; set; }
        public List<PeccancyInfo> PeccancyInfos { get; set; }

    }
}