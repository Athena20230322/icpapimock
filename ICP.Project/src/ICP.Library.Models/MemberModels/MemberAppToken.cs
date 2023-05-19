using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 會員App金鑰表
    /// </summary>
    public class MemberAppToken
    {
        /// <summary>
        /// AppToken 編號
        /// </summary>
        public long AppTokenID { get; set; }

        /// <summary>
        /// OPEN WALLET ID
        /// </summary>
        public string OPMID { get; set; }

        /// <summary>
        /// OPEN WALLET AuthV
        /// </summary>
        public string AuthV { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        public string OPAccessToken { get; set; }

        /// <summary>
        /// AccessToken 過期時間
        /// </summary>
        public DateTime OPExpired { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string OPCellPhone { get; set; }

        /// <summary>
        /// 手機條碼
        /// </summary>
        public string OPMobileBarcode { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 登入/註冊TokenID
        /// </summary>
        public string LoginTokenID { get; set; }

        /// <summary>
        /// 登入/註冊TokenID 期限
        /// </summary>
        public DateTime LoginTokenExpired { get; set; }
    }
}
