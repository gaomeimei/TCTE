using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TCTE.Models
{
    public class Province
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1)]
        public string Abbr { get; set; }

        // 导航属性
        public virtual ICollection<City> Cities { get; set; }
    }
}
