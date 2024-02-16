using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Infrastructure.Core.Models;

namespace ICP.Library.Models.OpenWalletApi.WebUIApi
{
    /// <summary>
    /// 會員登入驗證結果
    /// </summary>
    public class LoginWebUIResult: BaseWebUIApiResult
    {
        /// <summary>
        /// 登入Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 時間格式 yyyy/MM/dd HH:mm:ss
        /// </summary>
        public string TokenDate { get; set; }
    }
}
