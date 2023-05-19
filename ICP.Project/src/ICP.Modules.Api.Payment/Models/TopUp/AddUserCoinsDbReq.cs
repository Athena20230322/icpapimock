using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models.TopUp
{
    public class AddUserCoinsDbReq
    {
        /// <summary>
        /// 交易編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 特店編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 帳務類型
        /// </summary>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 帳務子類型
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 款項來源
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal TradeRealCash { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public decimal TradeTopUpCash { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Notes { get; set; }
    }
}
