using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.PaymentCenter
{
    /// <summary>
    /// 金流中心統計資訊
    /// </summary>
    public class TradeStatisticsModel : BaseListModel
    {
        /// <summary>
        /// 付款時間
        /// </summary>
        public DateTime PaymentDate { get; set; }

        #region 第一銀行
        /// <summary>
        /// ATM儲值筆數
        /// </summary>
        public int FirstATMTopupCount { get; set; }

        /// <summary>
        /// ATM儲值金額
        /// </summary>
        public decimal FirstATMTopupAmount { get; set; }

        /// <summary>
        /// AccountLink儲值筆數
        /// </summary>
        public int FirstACLTopupCount { get; set; }

        /// <summary>
        /// AccountLink儲值金額
        /// </summary>
        public decimal FirstACLTopupAmount { get; set; }

        /// <summary>
        /// AccountLink交易筆數
        /// </summary>
        public int FirstACLTradeCount { get; set; }

        /// <summary>
        /// AccountLink交易金額
        /// </summary>
        public decimal FirstACLTradeAmount { get; set; }
        #endregion

        #region 中國信託銀行
        /// <summary>
        /// ATM儲值筆數
        /// </summary>
        public int CtbcATMTopupCount { get; set; }

        /// <summary>
        /// ATM儲值金額
        /// </summary>
        public decimal CtbcATMTopupAmount { get; set; }

        /// <summary>
        /// AccountLink儲值筆數
        /// </summary>
        public int CtbcACLTopupCount { get; set; }

        /// <summary>
        /// AccountLink儲值金額
        /// </summary>
        public decimal CtbcACLTopupAmount { get; set; }

        /// <summary>
        /// AccountLink交易筆數
        /// </summary>
        public int CtbcACLTradeCount { get; set; }

        /// <summary>
        /// AccountLink交易金額
        /// </summary>
        public decimal CtbcACLTradeAmount { get; set; }
        #endregion

        #region 小計
        #region 第一銀行
        /// <summary>
        /// ATM儲值總筆數
        /// </summary>
        public int FirstATMTopupTotalCount { get; set; }

        /// <summary>
        /// ATM儲值總金額
        /// </summary>
        public decimal FirstATMTopupTotalAmount { get; set; }

        /// <summary>
        /// AccountLink儲值總筆數
        /// </summary>
        public int FirstACLTopupTotalCount { get; set; }

        /// <summary>
        /// AccountLink儲值總金額
        /// </summary>
        public decimal FirstACLTopupTotalAmount { get; set; }

        /// <summary>
        /// AccountLink交易總筆數
        /// </summary>
        public int FirstACLTradeTotalCount { get; set; }

        /// <summary>
        /// AccountLink交易總金額
        /// </summary>
        public decimal FirstACLTradeTotalAmount { get; set; }
        #endregion
        #region 中國信託銀行
        /// <summary>
        /// ATM儲值筆數
        /// </summary>
        public int CtbcATMTopupTotalCount { get; set; }

        /// <summary>
        /// ATM儲值金額
        /// </summary>
        public decimal CtbcATMTopupTotalAmount { get; set; }

        /// <summary>
        /// AccountLink儲值筆數
        /// </summary>
        public int CtbcACLTopupTotalCount { get; set; }

        /// <summary>
        /// AccountLink儲值金額
        /// </summary>
        public decimal CtbcACLTopupTotalAmount { get; set; }

        /// <summary>
        /// AccountLink交易筆數
        /// </summary>
        public int CtbcACLTradeTotalCount { get; set; }

        /// <summary>
        /// AccountLink交易金額
        /// </summary>
        public decimal CtbcACLTradeTotalAmount { get; set; }
        #endregion
        #endregion
    }
}
