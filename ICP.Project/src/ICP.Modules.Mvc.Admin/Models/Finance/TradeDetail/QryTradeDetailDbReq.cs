using System;

namespace ICP.Modules.Mvc.Admin.Models.Finance.TradeDetail
{
    /// <summary>
    /// 實質交易明細查詢 查詢條件
    /// </summary>
    public class QryTradeDetailDbReq
    {
        /// <summary>
        /// 日期條件
        /// </summary>
        /// <remarks>1:訂單日期(預設) 2:付款日期 3:傳輸日期 4:撥款日期 5:退款日期</remarks>
        public int DateType { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// 訂單類別
        /// </summary>
        /// <remarks>1:icashpay訂單編號(TradeNo) 2:特店訂單編號(MerchantTradeNo)</remarks>
        public int TradeNoType { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 付款狀態
        /// </summary>
        /// <remarks>0:全部(預設) 1:未付款 2:已付款 3:已退款 4:付款失敗 5:退款失敗</remarks>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        /// <remarks>0:全部(預設) 1:銷售(交易) 2:銷退(退款) 3:銷售/銷退 4:沖正</remarks>
        public int TradeStatus { get; set; }

        /// <summary>
        /// 電支帳號類型
        /// </summary>
        /// <remarks>1:收款方 2:付款方</remarks>
        public int ICPMIDType { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 撥款狀態
        /// </summary>
        /// <remarks>0:全部(預設) 1:未撥款 2:已撥款</remarks>
        public int AllocateStatus { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        /// <remarks>0:全部(預設) 1:icashpay帳戶 2:連結扣款帳戶</remarks>
        public int PaymentType { get; set; }

        /// <summary>
        /// 平台商編號
        /// </summary>
        public long? PlatformID { get; set; }

        /// <summary>
        /// 目前頁數
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 每頁顯示筆數
        /// </summary>
        public int PageSize { get; set; }
    }
}
