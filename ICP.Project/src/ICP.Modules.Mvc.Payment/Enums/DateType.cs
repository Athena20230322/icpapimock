using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Payment.Enums
{
    /// <summary>
    /// 日期區間類別
    /// </summary>
    public enum DateType
    {
        /// <summary>
        /// 今日
        /// </summary>
        Today = 0,
        /// <summary>
        /// 本週
        /// </summary>
        Week = 1,
        /// <summary>
        /// 本月
        /// </summary>
        Month = 2,
        /// <summary>
        /// 自訂
        /// </summary>
        Custom = 3
    }
}
