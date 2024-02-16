using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.PaymentCenter
{
    public class FeeStatisticsDetailModel : BaseListModel
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
        /// 交易序號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易/儲值日期
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// 交易/儲值金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 手續費/佣金費率( %數 / $筆)
        /// </summary>
        public string FeeRate { get; set; }

        /// <summary>
        /// 手續費/佣金
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal RefundAmount { get; set; }

        #region 小計
        /// <summary>
        /// 交易/儲值總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 手續費/佣金總金額
        /// </summary>
        public decimal TotalFee { get; set; }

        /// <summary>
        /// 退款總金額
        /// </summary>
        public decimal TotalRefundAmount { get; set; }
        #endregion
    }
}
