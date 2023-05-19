using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.AccountLink.Models.ChinaTrust
{
    public class ACLinkQueryReturnModel
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
        /// 電子支付帳戶
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 存款帳號
        /// </summary>
        public string DebitAccount { get; set; }

        /// <summary>
        /// 交易序號
        /// </summary>
        public string TrxNo { get; set; }

        /// <summary>
        /// 帳戶綁定日期(yyyyMMdd)
        /// </summary>
        public string TrxDate { get; set; }

        /// <summary>
        /// 帳戶綁定時間(hhMMss)
        /// </summary>
        public string TrxTime { get; set; }

        /// <summary>
        /// 綁定狀態(0: 正常綁定,1: 未綁定,2: 已銷戶)
        /// </summary>
        public string BindingStatus { get; set; }

        /// <summary>
        /// 回覆代碼
        /// </summary>
        public string ReturnCode { get; set; }

        /// <summary>
        /// 回覆訊息
        /// </summary>
        public string ReturnMessage { get; set; }

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
