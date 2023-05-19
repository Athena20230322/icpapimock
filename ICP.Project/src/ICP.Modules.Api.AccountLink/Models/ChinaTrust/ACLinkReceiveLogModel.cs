using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.AccountLink.Models.ChinaTrust
{
    public class ACLinkReceiveLogModel
    {
        /// <summary>
        /// Log類型
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        /// 中信交易序號
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 回覆代碼
        /// </summary>
        public string ReturnCode { get; set; }

        /// <summary>
        /// 回覆訊息
        /// </summary>
        public string ReturnMessage { get; set; }

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
        /// 會員生日
        /// </summary>
        public string Birth { get; set; }

        /// <summary>
        /// 存款帳號
        /// </summary>
        public string DebitAccount { get; set; }

        /// <summary>
        /// 交易序號
        /// </summary>
        public string TrxNo { get; set; }

        /// <summary>
        /// 交易日期時間
        /// </summary>
        public string TrxTime { get; set; }

        /// <summary>
        /// 網頁識別碼
        /// </summary>
        public string AuthId { get; set; }

        /// <summary>
        /// 綁定狀態(0: 正常綁定,1: 未綁定,2: 已銷戶)
        /// </summary>
        public string BindingStatus { get; set; }

        /// <summary>
        /// 原交易結果
        /// </summary>
        public string TrxResult { get; set; }

        /// <summary>
        /// CTBC Internal ErrorCode
        /// </summary>
        public string ServiceCode { get; set; }

        /// <summary>
        /// CTBC Internal ErrorMessage
        /// </summary>
        public string ServiceMessage { get; set; }
    }
}
