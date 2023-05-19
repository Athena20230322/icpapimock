using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels.Bonus
{
    public class QryBonusReq : PageModel
    {
        /// <summary>
        /// 日期類型 0:訂單日期 1:付款日期 2:退款日期
        /// </summary>
        public int DateType { get; set; }

        /// <summary>
        /// 查詢起日
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 查詢迄日
        /// </summary>
        public DateTime EndDate { get; set; }

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
        /// iCashPay訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 特店訂單編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 預留-紅利類型(0:全部 1:OPENPOINT)
        /// </summary>
        public int PointType { get; set; }
    }
}
