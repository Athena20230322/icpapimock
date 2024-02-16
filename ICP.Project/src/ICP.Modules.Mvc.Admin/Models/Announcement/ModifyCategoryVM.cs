using ICP.Infrastructure.Core.Models.Consts;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.Announcement
{
    /// <summary>
    /// 新增/修改 公告類別 ViewModel
    /// </summary>
    public class ModifyCategoryVM
    {
        /// <summary>
        /// 類別編號
        /// </summary>
        public int CategoryID { get; set; }

        [Display(Name = "公告類別名稱")]
        [Required(ErrorMessage = "請輸入 2-10 個字元，可中英數混合")]
        [RegularExpression(RegexConst.Ch_En_Number, ErrorMessage = "請輸入 2-10 個字元，可中英數混合")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "請輸入 2-10 個字元，可中英數混合")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 公告類別狀態
        /// </summary>
        /// <remarks>0:未啟用 1: 啟用 2: 刪除</remarks>
        [Display(Name = "狀態")]
        [Required(ErrorMessage = "請選擇訊息公告類別")]
        public int Status { get; set; }

        /// <summary>
        /// 顯示
        /// </summary>
        public byte Visible { get; set; }
    }
}
