using System;

namespace ICP.Modules.Mvc.Admin.Models.Finance.MerchantTradeDetail
{
    /// <summary>
    /// 特店帳務進出明細 查詢條件
    /// </summary>
    public class QryMerchantTradeDetailDbReq
    {
        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// 電支使用者
        /// </summary>
        /// <remarks>1:電支帳號 2:名稱</remarks>
        public int UserType { get; set; }

        /// <summary>
        /// 特店帳號/特店名稱(模糊搜尋)
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 帳務類型
        /// </summary>
        /// <remarks>0:全部(預設) 1:交易 2:儲值 3:轉帳 4:提領 5:撥款 6:調帳</remarks>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        /// <remarks>0:全部(預設)</remarks>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 交易子類型
        /// </summary>
        /// <remarks>0:全部(預設)</remarks>
        public int PaymentSubTypeID { get; set; }

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
