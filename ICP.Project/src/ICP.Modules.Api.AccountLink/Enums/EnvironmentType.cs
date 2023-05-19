using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.AccountLink.Enums
{
    /// <summary>
    /// 執行環境
    /// </summary>
    public enum EnvironmentType : int
    {
        /// <summary>
        /// 開發
        /// </summary>
        Dev = 1,
        /// <summary>
        /// 內部測試
        /// </summary>
        Beta = 2,
        /// <summary>
        /// 開放測試
        /// </summary>
        Stage = 3,
        /// <summary>
        /// 正式
        /// </summary>
        Prod = 4
    }
}
