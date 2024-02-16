using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.WebUIApi
{
    /// <summary>
    /// OP Web UI Token 資料
    /// </summary>
    public class MemberOPWebToken
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        public string OPAccessToken { get; set; }

        /// <summary>
        /// AccessToken 過期時間
        /// </summary>
        public DateTime OPExpired { get; set; }
    }
}
