using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.EinvoiceLibrary.Enum
{
    /// <summary>
    /// 電子發票載具驗證狀態
    /// </summary>
    public enum EinvoiceVerifyStatus
    {
        /// <summary>
        /// 通過
        /// </summary>
        Enable = 1,
        /// <summary>
        /// 未通過未驗證
        /// </summary>
        Disable = 0
    }
}
