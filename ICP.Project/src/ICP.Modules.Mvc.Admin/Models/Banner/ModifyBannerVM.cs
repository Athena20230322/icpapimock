using ICP.Infrastructure.Core.Models.Consts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.Banner
{
    /// <summary>
    /// 新增/修改 廣告 ViewModel
    /// </summary>
    public class ModifyBannerVM
    {
        #region 廣告基本設定
        /// <summary>
        /// 廣告位置選單
        /// </summary>
        [Display(Name = "廣告位置")]
        public List<BannerSiteModel> BannerSiteList { get; set; }

        [Display(Name = "廣告起訖時間")]
        [Required(ErrorMessage = "請設定廣告上下架時間")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string StartDate { get; set; }

       [Required(ErrorMessage = "請設定廣告上下架時間")]
        [RegularExpression(RegexConst.HHmm, ErrorMessage = "時間格式錯誤")]
        public string StartDateTime { get; set; }

        [Display(Name = "廣告起訖時間")]
        [Required(ErrorMessage = "請設定廣告上下架時間")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string EndDate { get; set; }

        [Required(ErrorMessage = "請設定廣告上下架時間")]
        [RegularExpression(RegexConst.HHmm, ErrorMessage = "時間格式錯誤")]
        public string EndDateTime { get; set; }
        #endregion

        #region 廣告內容設定
        /// <summary>
        /// 是否有第二層內容
        /// </summary>
        /// <remarks>0否 1是</remarks>
        [Display(Name = "是否有第二層內容")]
        [Required(ErrorMessage = "請選擇")]
        public int IsUseContent { get; set; }

        [Display(Name = "第一層廣告連結")]
        public string UrlLink1 { get; set; }

        [Display(Name = "圖片上傳")]
        [Required(ErrorMessage = "請上傳外層BANNER圖片")]
        public string ImagePath { get; set; }

        [Display(Name = "廣告排序")]
        [Required(ErrorMessage = "請輸入外層BANNER顯示排序")]
        public int? OrderID { get; set; }

        /// <summary>
        /// 頁面開啟設定
        /// </summary>
        /// <remarks>0內開 1外開</remarks>
        [Display(Name = "頁面開啟設定")]
        [Required(ErrorMessage = "請選擇內層連結設定之頁面開啟方式")]
        public int OpenNewWindow1 { get; set; }
        #endregion

        #region 廣告內頁
        [Display(Name = "廣告標題")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "請輸入2-20字活動名稱")]
        public string Title { get; set; }

        [Display(Name = "圖片上傳(非必填)")]
        public List<string> ImagePathList { get; set; }

        [AllowHtml]
        [Display(Name = "廣告內容")]
        [MinLength(10, ErrorMessage = "請輸入 10 - 1000 字廣告說明")]
        public string BannerContent { get; set; }

        [Display(Name = "廣告連結(非必填)")]
        public string UrlLink2 { get; set; }

        /// <summary>
        /// 頁面開啟設定
        /// </summary>
        /// <remarks>0內開 1外開</remarks>
        [Display(Name = "頁面開啟設定(非必填)")]
        public int? OpenNewWindow2{ get; set; }
        #endregion

        /// <summary>
        /// 廣告編號
        /// </summary>
        public int BannerID { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        /// <remarks>0:預設 1:審核通過 2: 退件</remarks>
        public int AuthStatus { get; set; }

        /// <summary>
        /// 內層BANNER圖片
        /// </summary>
        [Display(Name = "內層圖片(非必填)")]
        public string ImagePath2 { get; set; }
    }

    /// <summary>
    /// 廣告位置model
    /// </summary>
    public class BannerSiteModel
    {
        /// <summary>
        /// 位置編號
        /// </summary>
        public int SiteID { get; set; }

        /// <summary>
        /// 位置名稱
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 是否勾選
        /// </summary>
        public bool IsSelected { get; set; } = false;
    }
}
