using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Interface;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.ChargeBack
{
    public class ChargeBackRes : BaseResult, ICheckMacValueResult
    {
        /// <summary>
        /// 愛金卡交易序號
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// 退款時間
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string CheckMacValue { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        #region 儲值退貨專用

        /// <summary>
        /// 儲值退貨金額
        /// </summary>
        public decimal TopUpAmt { get; set; }

        #endregion
    }
}
