using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCTE.Models
{
    public class OrderImage
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string DecisionNumber { get; set; }

        public string ImageContent { get; set; }
    }
}