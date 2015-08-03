using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace TCTE.ViewModel
{
    public class TerminalInitViewModel
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string SalesManCode { get; set; }
        [Required]
        public string FingerPrint { get; set; }
    }
}