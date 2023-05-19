using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Payment.Models.Cashier
{
    public class CashierRes : BaseResult
    {
        /// <summary>
        /// 付款人會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易類型(EC:1 POS:2)
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// 交易模式(交易:1 儲值:2 轉帳:3 提領:4)
        /// </summary>
        public int TradeModeID { get; set; }

        // <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 付款時間
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string CheckMacValue { get; set; }

        /// <summary>
        /// 付款方式ID
        /// </summary>
        public string PayID { get; set; }

    }
}
