using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.Finance.MerchantTradeDetail
{
    /// <summary>
    /// 特店帳務進出明細 查詢條件
    /// </summary>
    public class QryMerchantTradeDetailReq : PageModel
    {
        /// <summary>
        /// 起始日期
        /// </summary>
        [Display(Name = "日期區間")]
        [Required(ErrorMessage = "請選擇日期")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string DateStart { get; set; } = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");

        /// <summary>
        /// 結束日期
        /// </summary>
        [Display(Name = "日期區間")]
        [Required(ErrorMessage = "請選擇日期")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string DateEnd { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        /// <summary>
        /// 電支使用者選單
        /// </summary>
        [Display(Name = "電支使用者")]
        public IEnumerable<SelectListItem> UserTypeList { get; set; }

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
        /// 帳務類型選單
        /// </summary>
        [Display(Name = "帳務類型")]
        public IEnumerable<SelectListItem> TradeModeList { get; set; }

        /// <summary>
        /// 帳務類型
        /// </summary>
        /// <remarks>0:全部(預設) 1:交易 2:儲值 3:轉帳 4:提領 5:撥款 6:調帳</remarks>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 交易類型選單
        /// </summary>
        [Display(Name = "交易類型")]
        public IEnumerable<SelectListItem> PaymentTypeList { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        /// <remarks>0:全部(預設)</remarks>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 交易子類型選單
        /// </summary>
        [Display(Name = "交易子類型")]
        public IEnumerable<SelectListItem> PaymentSubTypeList { get; set; }

        /// <summary>
        /// 交易子類型
        /// </summary>
        /// <remarks>0:全部(預設)</remarks>
        public int PaymentSubTypeID { get; set; }

        public new int PageSize { get; set; } = 10;
    }
}
