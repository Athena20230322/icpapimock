using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels.Bonus
{
    public class QryBonusRes : BaseListModel
    {
        /// <summary>
        /// 預留-紅利類型(0:全部 1:OPENPOINT)
        /// </summary>
        public int PointType { get; set; }

        /// <summary>
        /// 訂單日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 退款日期
        /// </summary>
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// iCashPay訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 特店訂單編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 收款方電支帳號
        /// </summary>
        public string SellerICPMID { get; set; }

        /// <summary>
        /// 收款方名稱
        /// </summary>
        public string SellerCName { get; set; }

        /// <summary>
        /// 收款方電支帳號
        /// </summary>
        public string BuyerICPMID { get; set; }

        /// <summary>
        /// 收款方名稱
        /// </summary>
        public string BuyerCName { get; set; }

        /// <summary>
        /// 訂單金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 折抵點數
        /// </summary>
        public decimal DebitPoint { get; set; }

        /// <summary>
        /// 點數折抵金額
        /// </summary>
        public decimal BonusAmt { get; set; }

        /// <summary>
        /// 實付/退金額
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款狀態
        /// </summary>
        public int RefundStatus { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public int PaymentTypeID { get; set; }
    }
}
