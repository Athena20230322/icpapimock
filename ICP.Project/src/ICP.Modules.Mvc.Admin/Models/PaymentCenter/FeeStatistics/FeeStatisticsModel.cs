using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.PaymentCenter
{
    public class FeeStatisticsModel : BaseListModel
    {
        /// <summary>
        /// 撥款時間
        /// </summary>
        public DateTime AllocateDate { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 特店名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        public string UnifiedBusinessNo { get; set; }

        /// <summary>
        /// 金流方式
        /// </summary>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 金流方式名稱
        /// </summary>
        public string TradeModeName { get { return TradeModeID == 1 ? "交易" : "儲值"; } }

        /// <summary>
        /// 交易/儲值筆數
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 交易/儲值金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 交易手續費(%數 / $筆)
        /// </summary>
        public string FeeRate { get; set; }

        /// <summary>
        /// 交易手續費金額
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 撥款淨額
        /// </summary>
        public decimal AllocateAmount { get; set; }

        /// <summary>
        /// 應收淨額
        /// </summary>
        public decimal ReceivableAmount { get; set; }

        /// <summary>
        /// 儲值佣金費率
        /// </summary>
        public string TopupBrokerageRate { get; set; }

        /// <summary>
        /// 儲值佣金
        /// </summary>
        public decimal TopupBrokerageAmount { get; set; }

        #region 小計
        /// <summary>
        /// 交易/儲值總筆數
        /// </summary>
        public int TotalTradeCount { get; set; }
        
        /// <summary>
        /// 交易/儲值總金額
        /// </summary>
        public decimal TotalTradeAmount { get; set; }

        /// <summary>
        /// 交易手續費總金額
        /// </summary>
        public decimal TotalChargeFee { get; set; }

        /// <summary>
        /// 退款總金額
        /// </summary>
        public decimal TotalRefundAmount { get; set; }

        /// <summary>
        /// 撥款總淨額
        /// </summary>
        public decimal TotalAllocateAmount { get; set; }

        /// <summary>
        /// 應收總淨額
        /// </summary>
        public decimal TotalReceivableAmount { get; set; }

        /// <summary>
        /// 儲值總佣金
        /// </summary>
        public decimal TotalTopupBrokerageAmount { get; set; }
        #endregion
    }
}
