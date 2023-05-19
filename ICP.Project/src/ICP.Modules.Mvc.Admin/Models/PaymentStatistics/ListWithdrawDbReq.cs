using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics
{
    /// <summary>
    /// 每日提領金額監控清單Req
    /// </summary>
    public class ListWithdrawDbReq : PageModel
    {
        /// <summary>
        /// 查詢日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public long? MID { get; set; }

        /// <summary>
        /// 商戶名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 提領金額(以上)
        /// </summary>
        public int? WithdrawAmount { get; set; }

        /// <summary>
        /// 僅顯示觀察廠商
        /// </summary>
        /// <remarks>0:否 1:是</remarks>
        public int MonitorStaus { get; set; } = 0;

        /// <summary>
        /// 排序方式(遞減)
        /// </summary>
        /// <remarks>1:選擇日期提領百分比 2:選擇日期提領金額 3:30天累計提領金額</remarks>
        public int SortType { get; set; }

        /// <summary>
        /// 提領設定
        /// </summary>
        /// <remarks>0:全部 1:手動 2:自動</remarks>
        public int TransferType { get; set; } = 0;

        /// <summary>
        /// 商品類別
        /// </summary>
        /// <remarks>0:全部 1:實體 2:虛擬 4:遞延 8:其他</remarks>
        public int CommoditiyType { get; set; } = 0;

        /// <summary>
        /// 筆數(以上)
        /// </summary>
        public int? WithdrawCount { get; set; }

        /// <summary>
        /// 查核類型
        /// </summary>
        /// <remarks>1:提領金額 2:30天累計提領金額</remarks>
        public int TradeType { get; set; }

        /// <summary>
        /// 查詢規則
        /// </summary>
        /// <remarks>1:規則一 2:規則二</remarks>
        public int RuleMode { get; set; }

        /// <summary>
        /// 前七天綁定銀行帳戶數
        /// </summary>
        public int? Day7TransferCount { get; set; }
    }
}
