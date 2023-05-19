using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    using Infrastructure.Core.Models;

    public class TradeResModel
    {
        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long PaymentCenterTradeID { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string CheckMacValue { get; set; }

        /// <summary>
        /// AccountLink付款回傳Code
        /// </summary>
        public string PayRtnCode { get; set; }

        /// <summary>
        /// AccountLink付款回傳Msg
        /// </summary>
        public string PayRtnMsg { get; set; }

        /// <summary>
        /// AccountLink驗證回傳Code
        /// </summary>
        public string VerifyRtnCode { get; set; }

        /// <summary>
        /// AccountLink驗證回傳Msg
        /// </summary>
        public string VerifyRtnMsg { get; set; }

        /// <summary>
        /// 虛擬帳號
        /// </summary>
        public string VirtualAccount { get; set; }

        /// <summary>
        /// 銀行交易序號
        /// </summary>
        public string BankTradeNo { get; set; }

        /// <summary>
        /// ATM回傳日期
        /// </summary>
        public DateTime ATMTradeDate { get; set; }

        /// <summary>
        /// ATM有效日期
        /// </summary>
        public DateTime ATMExpireDate { get; set; }

        /*
        /// <summary>
        /// 原始金額
        /// </summary>
        public decimal OriginalAmt { get; set; }

        /// <summary>
        /// 儲值後金額
        /// </summary>
        public decimal CurrentAmt { get; set; }

        /// <summary>
        /// 儲值日期
        /// </summary>
        public string TopUpDate { get; set; }

        /// <summary>
        /// 可儲值額度
        /// </summary>
        public decimal TopUpAmtAvailable { get; set; }
        */
    }
}
