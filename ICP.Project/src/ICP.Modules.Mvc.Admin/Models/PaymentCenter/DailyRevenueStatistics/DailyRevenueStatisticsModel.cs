using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.PaymentCenter
{
    /// <summary>
    /// 每日營收統計資訊
    /// </summary>
    public class DailyRevenueStatisticsModel : BaseListModel
    {
        /// <summary>
        /// 付款時間
        /// </summary>
        public DateTime PaymentDate { get; set; }

        #region 統計
        /// <summary>
        /// 電支帳戶交易筆數
        /// </summary>
        public int ICashCount { get; set; }

        /// <summary>
        /// 電支帳戶交易金額
        /// </summary>
        public decimal ICashAmount { get; set; }

        /// <summary>
        /// 電支帳戶手續費
        /// </summary>
        public decimal ICashFee { get; set; }

        /// <summary>
        /// 連結帳戶交易筆數
        /// </summary>
        public int ACLCount { get; set; }

        /// <summary>
        /// 連結帳戶交易金額
        /// </summary>
        public decimal ACLAmount { get; set; }

        /// <summary>
        /// 連結帳戶手續費
        /// </summary>
        public decimal ACLFee { get; set; }
        #endregion

        #region 小計
        /// <summary>
        /// 電支帳戶交易總筆數
        /// </summary>
        public int ICashTotalCount { get; set; }

        /// <summary>
        /// 電支帳戶交易總金額
        /// </summary>
        public decimal ICashTotalAmount { get; set; }

        /// <summary>
        /// 電支帳戶總手續費
        /// </summary>
        public decimal ICashTotalFee { get; set; }

        /// <summary>
        /// 連結帳戶交易總筆數
        /// </summary>
        public int ACLTotalCount { get; set; }

        /// <summary>
        /// 連結帳戶交易總金額
        /// </summary>
        public decimal ACLTotalAmount { get; set; }

        /// <summary>
        /// 連結帳戶總手續費
        /// </summary>
        public decimal ACLTotalFee { get; set; }
        #endregion
    }
}
