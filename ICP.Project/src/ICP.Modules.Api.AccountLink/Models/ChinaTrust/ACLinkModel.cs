using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.AccountLink.Models.ChinaTrust
{
    public class ACLinkModel
    {
        /// <summary>
        /// 中信交易序號
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 業種(01: 電子支付)
        /// </summary>
        public string MerchantType { get; set; }

        /// <summary>
        /// 業者統編
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// 連結類別
        /// </summary>
        public string LinkType { get; set; }

        /// <summary>
        /// 電子支付帳戶
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 連結會員關係 (電子支付固定帶00)
        /// </summary>
        public string HolderRelationship { get; set; }

        /// <summary>
        /// 存款帳號
        /// </summary>
        public string DebitAccount { get; set; }

        /// <summary>
        /// 網頁識別碼
        /// </summary>
        public string AuthId { get; set; }

        /// <summary>
        /// OTP驗證碼
        /// </summary>
        public string Otp { get; set; }

        /// <summary>
        /// 會員生日
        /// </summary>
        public string Birth { get; set; }

        /// <summary>
        /// 交易序號
        /// </summary>
        public string TrxNo { get; set; }
    }
}
