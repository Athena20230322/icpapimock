using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class TradeInfoAtm : BaseResult
    {
        /// <summary>
        /// 虛擬帳號
        /// </summary>
        public long VirtualAccount { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 廠商訂單編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易編號(from Payment)
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 付款日期 → 格式：yyyyMMdd
        /// </summary>
        public string PaymentDate { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal TradeAMT { get; set; }

        /// <summary>
        /// 回傳網址
        /// </summary>
        public string ReplyURL { get; set; }

        /// <summary>
        /// 回傳銀行代碼
        /// </summary>
        public string RtnBankCode { get; set; }

        /// <summary>
        /// 回傳銀行帳號
        /// </summary>
        public string RtnBankAcc { get; set; }

        /// <summary>
        /// 付款金額
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 有效日期(轉帳期限)
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 收單銀行代碼
        /// </summary>
        public string BankCode { get; set; }
    }
}
