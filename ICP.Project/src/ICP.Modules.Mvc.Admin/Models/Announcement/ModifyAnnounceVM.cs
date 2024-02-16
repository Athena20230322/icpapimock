using ICP.Infrastructure.Core.Models.Consts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.Announcement
{
    /// <summary>
    /// 新增/修改 公告 ViewModel
    /// </summary>
    public class ModifyAnnounceVM
    {
        #region 訊息公告基本設定
        [Display(Name = "公告類別ID")]
        [Required(ErrorMessage = "請先選擇訊息公告類別")]
        public string CategoryID { get; set; }

        [Display(Name = "訊息公告類別")]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [Display(Name = "訊息公告上線時間")]
        [Required(ErrorMessage = "請選擇欲上線日期")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "請選擇欲上線時間")]
        [RegularExpression(RegexConst.HHmm, ErrorMessage = "時間格式錯誤")]
        public string StartDateTime { get; set; }

        [Display(Name = "是否置頂")]
        public int IsTop { get; set; }

        [Display(Name = "置頂起訖時間")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string IsTopStartDate { get; set; }

        [RegularExpression(RegexConst.HHmm, ErrorMessage = "時間格式錯誤")]
        public string IsTopStartDateTime { get; set; }

        [Display(Name = "置頂起訖時間")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string IsTopEndDate { get; set; }

        [RegularExpression(RegexConst.HHmm, ErrorMessage = "時間格式錯誤")]
        public string IsTopEndDateTime { get; set; }
        #endregion

        #region 訊息公告內容設定
        [Display(Name = "訊息公告標題")]
        [Required(ErrorMessage = "請輸入 2-20 中英數字標題")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "請輸入 2-20 中英數字標題")]
        public string Title { get; set; }

        [Display(Name = "圖片上傳")]
        public List<string> ImagePathList { get; set; }

        [AllowHtml]
        [Display(Name = "訊息公告內容")]
        [Required(ErrorMessage = "請輸入10-1000字內容")]
        [MinLength(10, ErrorMessage = "請輸入10-1000字內容")]
        public string AnnounceContent { get; set; }
        #endregion

        #region 訊息公告發送設定
        [Display(Name = "發送對象")]
        public int AnnounceType { get; set; }

        [Display(Name = "發送對象")]
        public IEnumerable<SelectListItem> AnnounceTypeList { get; set; }

        [Display(Name = "上傳發送名單")]
        public List<MidFileModel> CsvPathList { get; set; }

        [Display(Name = "測試發送")]
        public string TestMidList { get; set; }

        /// <summary>
        /// 是否已測試發送
        /// </summary>
        public bool IsTest { get; set; }
        #endregion

        [Display(Name = "訊息公告類別")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 公告編號
        /// </summary>
        public int NID { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        /// <remarks>0:待審核 1:審核通過 2: 退件</remarks>
        public int AuthStatus { get; set; }
    }

    public class MidFileModel
    {
        /// <summary>
        /// 檔案編號
        /// </summary>
        public int FileID { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        /// <remarks>0: 刪除 1: 正常</remarks>
        public int Status { get; set; }
    }
}
