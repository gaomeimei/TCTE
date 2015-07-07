using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCTE.Models.SystemType
{
    public enum TerminalStatus
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        NotInitialized = 1,
        /// <summary>
        /// 设备正常
        /// </summary>
        [Display(Name = "正常")]
        Normal = 2,
        /// <summary>
        /// 设备故障
        /// </summary>
        [Display(Name = "故障")]
        Malfunction = 3
    }
}