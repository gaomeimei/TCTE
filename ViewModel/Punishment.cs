using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCTE.ViewModel
{
    public class Punishment
    {
        public string DecisionNumber { get; set; }
        public DateTime? PeccancyTime { get; set; }
        public string PeccancyAddress { get; set; }
        public string PeccancyBehavior { get; set; }
        public int Deduction { get; set; }
        public decimal? Money { get; set; }
        public string OrderCode { get; set; }
    }
}