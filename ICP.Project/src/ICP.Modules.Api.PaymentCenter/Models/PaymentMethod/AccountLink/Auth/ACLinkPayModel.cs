using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink.Auth
{
    /// <summary>
    /// 第一銀行-連結帳戶交易扣款(ACLinkPay) 接收參數
    /// </summary>
    public class FirstACLinkPayModel : BaseACLinkModel
    {
        /// <summary>
        /// 交易訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public string TradeTime { get; set; }

        /// <summary>
        /// 交易明細
        /// </summary>
        public string TradeNote { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int TradeAmt { get; set; }

        /// <summary>
        /// 轉入虛擬帳戶
        /// </summary>
        public string PayeeAccount { get; set; }
    }

    /// <summary>
    /// 國泰世華-連結帳戶交易扣款(ACLinkPay) 接收參數
    /// </summary>
    public class CathayACLinkPayModel : BaseACLinkModel
    {
        /// <summary>
        /// 銀行帳號
        /// </summary>
        //public string BankAccount { get; set; }

        /// <summary>
        /// 扣款金額
        /// </summary>        
        public int TradeAmt { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string TradeNo { get; set; }
    }

    /// <summary>
    /// 中國信託-連結帳戶交易扣款(ACLinkPay) 接收參數
    /// </summary>
    public class ChinaTrustACLinkPayModel : BaseACLinkModel
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易模式
        /// </summary>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 扣款金額
        /// </summary>        
        public int TradeAmt { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public string TradeTime { get; set; }

        /// <summary>
        /// 交易備註
        /// </summary>
        public string TradeNote { get; set; }
    }
}
