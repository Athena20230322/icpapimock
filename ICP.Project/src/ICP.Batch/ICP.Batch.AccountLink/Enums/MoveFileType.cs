using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.AccountLink.Enums
{
    /// <summary>
    /// 檔案搬移目標類型
    /// </summary>
    public enum MoveFileType
    {
        /// <summary>
        /// 壓縮檔
        /// </summary>
        Zip = 1,
        /// <summary>
        /// 進行中的檔案
        /// </summary>
        Process = 2,
        /// <summary>
        /// 執行成功的檔案
        /// </summary>
        Success = 3,
        /// <summary>
        /// 執行失敗的檔案
        /// </summary>
        Fail = 4,
        /// <summary>
        /// 解密搬移失敗的檔案
        /// </summary>
        MoveFail = 5,
        /// <summary>
        /// 執行上傳的檔案
        /// </summary>
        Upload = 6
    }
}
