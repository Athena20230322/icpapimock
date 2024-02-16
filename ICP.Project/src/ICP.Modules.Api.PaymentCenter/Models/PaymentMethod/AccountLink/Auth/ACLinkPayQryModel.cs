using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink.Auth
{
    /// <summary>
    /// 第一銀行-連結帳戶交易查詢(ACLinkPayQuery) 接收參數
    /// </summary>
    public class FisrtACLinkPayQryModel : BaseACLinkModel
    {
        /// <summary>
        /// 查詢訊息序號
        /// </summary>
        public string SerMsgNo { get; set; }
    }

    /// <summary>
    /// 中國信託-連結帳戶交易查詢(ACLinkPayQuery) 接收參數
    /// </summary>
    public class ChinaTrustACLinkPayQryModel : BaseACLinkModel
    {
        /// <summary>
        /// 與銀行交易的原始交易序號
        /// </summary>
        public string BankTradeNo { get; set; }
    }

    /// <summary>
    /// 國泰世華-連結帳戶交易扣款(ACLinkPayQuery) 接收參數
    /// </summary>
    public class CathayACLinkPayQryModel : BaseACLinkModel
    {
        /// <summary>
        /// 與銀行交易的原始交易序號
        /// </summary>
        public string BankTradeNo { get; set; }
    }
}
