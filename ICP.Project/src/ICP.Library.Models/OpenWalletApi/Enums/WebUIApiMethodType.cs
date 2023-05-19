using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.Enums
{
    /// <summary>
    /// OP Web Api (與外包 Web UI 廠商的API串接方法)
    /// </summary>
    public enum WebUIApiMethodType
    {
        None =0,

        /// <summary>
        /// icash會員登入驗證
        /// </summary>
        Login = 1,

        /// <summary>
        /// 身分驗證
        /// </summary>
        Verify = 2,

        /// <summary>
        /// 查詢未成年身分資料
        /// </summary>
        GetUserData = 3,

        /// <summary>
        /// ICP004 同意未成年註冊				
        /// </summary>
        AgreeRegister = 4,

        /// <summary>
        /// 廣告取得
        /// </summary>
        GetAD = 5,

        /// <summary>
        /// 今日廣告最後異動時間
        /// </summary>
        GetADLastTime = 6
    }
}
