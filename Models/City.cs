using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace TCTE.Models
{
    public class City
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "城市")]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "城市简称")]
        public string Abbr { get; set; }

        [Required, Display(Name = "省份")]
        public int ProvinceId { get; set; }

        // 导航属性
        public virtual Province Provice { get; set; }
    }
}
