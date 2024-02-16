using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink.Auth
{
    /// <summary>
    /// 第一銀行-連結帳戶退款(ACLinkRefund) 接收參數
    /// </summary>
    public class FisrtACLinkRefundModel : BaseACLinkModel
    {
        /// <summary>
        /// 退款(原)訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 退款交易時間
        /// </summary>
        public string RefundTime { get; set; }

        /// <summary>
        /// 退款交易明細
        /// </summary>
        public string RefundNote { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public int RefundAmt { get; set; }
    }

    /// <summary>
    /// 中國信託-連結帳戶退款(ACLinkRefund) 接收參數
    /// </summary>
    public class ChinaTrustACLinkRefundModel : BaseACLinkModel
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 與銀行交易的原始交易序號
        /// </summary>
        public string BankTradeNo { get; set; }

        /// <summary>
        /// 來源(1:交易 3:儲值)
        /// </summary>
        public int TradeSource { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public int RefundAmt { get; set; }

        /// <summary>
        /// 退款時間
        /// </summary>
        public string RefundTime { get; set; }

        /// <summary>
        /// 交易備註
        /// </summary>
        public string TradeNote { get; set; }
    }

    /// <summary>
    /// 國泰世華-連結帳戶退款(ACLinkRefund) 接收參數
    /// </summary>
    public class CathayACLinkRefundModel : BaseACLinkModel
    {
        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 訊息序號
        /// </summary>
        public string MsgNo { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>        
        public int RefundAmt { get; set; }

        /// <summary>
        /// 退款時間
        /// </summary>
        public string RefundTime { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }
    }
}
