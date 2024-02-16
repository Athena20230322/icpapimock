using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.WebUIApi
{
    /// <summary>
    /// 登入, 若無待審的未成年資料時無法登入
    /// </summary>
    public class LoginWebUIRequest: BaseWebUIApiRequest
    {
        /// <summary>
        /// 登入帳號
        /// </summary>
        public string LoginID { get; set; }

        /// <summary>
        /// 登入密碼
        /// </summary>
        public string PassWord { get; set; }
    }
}