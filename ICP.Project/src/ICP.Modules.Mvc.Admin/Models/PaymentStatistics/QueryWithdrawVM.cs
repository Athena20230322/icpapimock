using ICP.Infrastructure.Core.Models.Consts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics
{
    /// <summary>
    /// 每日提領金額監控 查詢ViewModel
    /// </summary>
    public class QueryWithdrawVM
    {
        [Display(Name = "查詢日期")]
        [Required(ErrorMessage = "請選擇日期")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string StartDate { get; set; }

        /// <summary>
        /// 顯示全部廠商/僅顯示觀察廠商
        /// </summary>
        public SelectType SelectTypeMode { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        [Display(Name = "電支帳號")]
        [RegularExpression(RegexConst.ICPMID, ErrorMessage = "請輸入正確的電支帳號")]
        public long? MID { get; set; }

        /// <summary>
        /// 商戶名稱
        /// </summary>
        [Display(Name = "商戶名稱")]
        public string MerchantName { get; set; }

        /// <summary>
        /// 查詢規則
        /// </summary>
        /// <remarks>1:規則一 2:規則二</remarks>
        public int RuleMode { get; set; }

        /// <summary>
        /// 查核類型
        /// </summary>
        /// <remarks>1:提領金額 2:30天累計提領金額</remarks>
        [Display(Name = "查核類型")]
        public int TradeType { get; set; }

        /// <summary>
        /// 查核類型選單
        /// </summary>
        /// <remarks>1:提領金額 2:30天累計提領金額</remarks>
        [Display(Name = "查核類型")]
        public IEnumerable<SelectListItem> TradeTypeList { get; set; }

        /// <summary>
        /// 排序方式(規則一)
        /// </summary>
        /// <remarks>1:選擇日期提領百分比 2:選擇日期提領金額 3:30天累計提領金額</remarks>
        [Display(Name = "排序方式")]
        public int SortType1 { get; set; }

        /// <summary>
        /// 排序方式(規則二)
        /// </summary>
        /// <remarks>1:選擇日期提領百分比 2:選擇日期提領金額 3:30天累計提領金額</remarks>
        [Display(Name = "排序方式")]
        public int SortType2 { get; set; }

        /// <summary>
        /// 排序方式選單
        /// </summary>
        /// <remarks>1:選擇日期提領百分比 2:選擇日期提領金額 3:30天累計提領金額</remarks>
        [Display(Name = "排序方式")]
        public IEnumerable<SelectListItem> SortTypeList { get; set; }

        /// <summary>
        /// 提領金額(以上)
        /// </summary>
        [Display(Name = "金額區間")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "金額區間數字格式錯誤")]
        public int? WithdrawAmount { get; set; }

        /// <summary>
        /// 筆數(以上)
        /// </summary>
        [Display(Name = "金額區間")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "金額區間數字格式錯誤")]
        public int? WithdrawCount { get; set; }

        /// <summary>
        /// 提領設定(規則一)
        /// </summary>
        [Display(Name = "提領設定")]
        public TransferType TransferTypeMode1 { get; set; }

        /// <summary>
        /// 提領設定(規則二)
        /// </summary>
        [Display(Name = "提領設定")]
        public TransferType TransferTypeMode2 { get; set; }

        /// <summary>
        /// 商品類別(規則一)
        /// </summary>
        [Display(Name = "商品類別")]
        public CommoditiyType CommoditiyTypeMode1 { get; set; }

        /// <summary>
        /// 商品類別(規則二)
        /// </summary>
        [Display(Name = "商品類別")]
        public CommoditiyType CommoditiyTypeMode2 { get; set; }

        /// <summary>
        /// 前七天綁定銀行帳戶數
        /// </summary>
        [Display(Name = "前七天綁定銀行帳戶數")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "前七天綁定銀行帳戶數數字格式錯誤")]
        public int? Day7TransferCount { get; set; }
    }

    /// <summary>
    /// 顯示全部廠商/僅顯示觀察廠商
    /// </summary>
    public class SelectType
    {
        /// <summary>
        /// 1:顯示全部廠商
        /// </summary>
        [Display(Name = "顯示全部商戶")]
        public bool SelectMode { get; set; }

        /// <summary>
        /// 2:僅顯示交易觀察名單
        /// </summary>
        [Display(Name = "僅顯示交易觀察名單")]
        public bool MonitorStaus { get; set; }
    }

    /// <summary>
    /// 提領設定
    /// </summary>
    public class TransferType
    {
        /// <summary>
        /// 1:手動
        /// </summary>
        [Display(Name = "手動")]
        public bool Manually { get; set; }

        /// <summary>
        /// 2:自動
        /// </summary>
        [Display(Name = "自動")]
        public bool Auto { get; set; }
    }

    /// <summary>
    /// 商品類別
    /// </summary>
    public class CommoditiyType
    {
        /// <summary>
        /// 1:實體
        /// </summary>
        [Display(Name = "實體")]
        public bool Real { get; set; }

        /// <summary>
        /// 2:虛擬
        /// </summary>
        [Display(Name = "虛擬")]
        public bool Virtual { get; set; }

        /// <summary>
        /// 4:遞延
        /// </summary>
        [Display(Name = "遞延")]
        public bool Extend { get; set; }

        /// <summary>
        /// 8:其他
        /// </summary>
        [Display(Name = "其他")]
        public bool Other { get; set; }
    }
}