using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCTE.Models;
namespace TCTE.ViewModel
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public List<OrderImage> OrderImages { get; set; }
    }
}