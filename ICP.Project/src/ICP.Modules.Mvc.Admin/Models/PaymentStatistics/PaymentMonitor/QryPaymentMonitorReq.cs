using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics.PaymentMonitor
{
    /// <summary>
    /// 每日付款交易金額監控 查詢條件
    /// </summary>
    public class QryPaymentMonitorReq : PageModel
    {
        /// <summary>
        /// 查詢日期
        /// </summary>
        [Display(Name = "查詢日期")]
        [Required(ErrorMessage = "請選擇日期")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string Date { get; set; } = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

        /// <summary>
        /// 電支帳號
        /// </summary>
        [Display(Name = "電支帳號")]
        [RegularExpression(RegexConst.ICPMID, ErrorMessage = "請輸入正確的電支帳號")]
        public string ICPMID { get; set; }

        /// <summary>
        /// 商戶名稱
        /// </summary>
        [Display(Name = "商戶名稱")]
        public string MerchantName { get; set; }

        /// <summary>
        /// 收款觀察名單(定時監控)
        /// </summary>
        /// <remarks>0:否 1:是</remarks>
        [Display(Name = "收款觀察名單")]
        public bool IncomeStaus { get; set; }

        /// <summary>
        /// 付款觀察名單
        /// </summary>
        /// <remarks>0:否 1:是</remarks>
        [Display(Name = "付款觀察名單")]
        public bool PaymentStatus { get; set; }

        /// <summary>
        /// 金流類型選單
        /// </summary>
        [Display(Name = "金流類型")]
        public IEnumerable<SelectListItem> TradeTypeList { get; set; }

        /// <summary>
        /// 金流類型
        /// </summary>
        /// <remarks>1:帳戶餘額 2:連結銀行帳號 3:1天總付款額 4:10天總付款額 5:30天總付款額 6:10天帳戶餘額付款 7:30天帳戶餘額付款 8:10天儲值總額</remarks>
        public int TradeType { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        [Display(Name = "類　　型")]
        public MerchantType MerchantTypeChkBox { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        /// <remarks>0:全選 1:個人 2:法人</remarks>
        public int MerchantType { get; set; }

        /// <summary>
        /// 金額區間-金額
        /// </summary>
        [Display(Name = "金額區間")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "金額格式錯誤")]
        public int Amount { get; set; } = 0;

        /// <summary>
        /// 金額區間-筆數
        /// </summary>
        [Display(Name = "金額區間")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "筆數格式錯誤")]
        public int Count { get; set; } = 0;

        /// <summary>
        /// 排序方式選單
        /// </summary>
        [Display(Name = "排序方式")]
        public IEnumerable<SelectListItem> SortTypeList { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        /// <remarks>1:帳戶餘額 2:連結銀行帳號 3:1天總付款額 4:10天總付款額 5:30天總付款額 6:10天帳戶餘額付款 7:30天帳戶餘額付款 8:10天儲值總額</remarks>
        public int SortType { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        /// <remarks>1:金額 2:筆數</remarks>
        public SortKind SortKindRdo { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        /// <remarks>1:金額 2:筆數</remarks>
        public int SortKind { get; set; }

        public new int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// 類型
    /// </summary>
    public class MerchantType
    {
        /// <summary>
        /// 個人
        /// </summary>
        [Display(Name = "個人")]
        public bool Personal { get; set; }

        /// <summary>
        /// 法人
        /// </summary>
        [Display(Name = "法人")]
        public bool LegalPerson { get; set; }
    }

    /// <summary>
    /// 排序方式
    /// </summary>
    public class SortKind
    {
        /// <summary>
        /// 金額
        /// </summary>
        [Display(Name = "金額")]
        public bool Amount { get; set; }

        /// <summary>
        /// 筆數
        /// </summary>
        [Display(Name = "筆數")]
        public bool Count { get; set; }
    }
}
