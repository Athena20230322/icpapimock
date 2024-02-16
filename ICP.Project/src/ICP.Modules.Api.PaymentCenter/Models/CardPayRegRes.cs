using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    /// <summary>
    /// 第一銀行 AP to AP 交易的 response model
    /// </summary>
    public class CardPayRegRes : CardPayRegBase
    {
        /// <summary>
        /// 回覆碼
        /// </summary>
        public string RC { get; set; }

        /// <summary>
        /// 回覆碼說明
        /// </summary>
        public string MSG { get; set; }

        /// <summary>
        /// 交易日期 → 西元yyyymmdd
        /// </summary>
        public string TxnDate { get; set; }

        /// <summary>
        /// 交易時間 → hhmmss
        /// </summary>
        public string TxnTime { get; set; }

        /// <summary>
        /// 押碼 → 從銀行端回傳
        /// </summary>
        public string MAC { get; set; }

        /// <summary>
        /// 交易時間(完整日期格式)
        /// </summary>
        [JsonIgnore]
        public DateTime TxnDateTime
        {
            get
            {
                if (!string.IsNullOrEmpty(TxnDate) && !string.IsNullOrEmpty(TxnTime))
                {
                    string ap2apTxnDate = $"{TxnDate.Substring(0, 4)}/{TxnDate.Substring(4, 2)}/{TxnDate.Substring(6, 2)}";
                    string ap2apTxnTime = $"{TxnTime.Substring(0, 2)}:{TxnTime.Substring(2, 2)}:{TxnTime.Substring(4, 2)}";
                    bool isRightTxnDateTime = DateTime.TryParse($"{ap2apTxnDate} {ap2apTxnTime}", out DateTime ap2apTxnDateTime);
                    return ap2apTxnDateTime;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
    }

    /// <summary>
    /// for XML 轉換用
    /// </summary>
    public class XmlTranserToCardPayRegRes
    {
        public CardPayRegRes CardPayRegRs { get; set; }
    }
}
