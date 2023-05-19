using ICP.Infrastructure.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Payment.Models.BaseMember
{
    /// <summary>
    /// 帳戶資訊>帳戶明細 請求
    /// </summary>
    public class AccountRecordReq : ValidatableObject
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 帳戶紀錄類別
        /// </summary>
        [Required(ErrorMessage = "{0} 欄位不能為空")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} 欄位格式不正確")]
        [Display(Name = "查詢類別")]
        public int AccRecordType { get; set; }

        /// <summary>
        /// 帳戶查詢區間類別
        /// </summary>
        [Required(ErrorMessage = "{0} 欄位不能為空")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} 欄位格式不正確")]
        [Display(Name = "查詢區間")]
        public int DateType { get; set; }

        /// <summary>
        /// 查詢起始日期
        /// </summary>
        [Display(Name = "起始日")]
        public string StartDate { get; set; }

        /// <summary>
        /// 查詢結束日期
        /// </summary>
        [Display(Name = "結束日")]
        public string EndDate { get; set; }

        /// <summary>
        /// 查詢關鍵字
        /// </summary>
        [Display(Name = "查詢關鍵字")]
        public string KeyWords { get; set; }

        /// <summary>
        /// 資料行檢核
        /// </summary>
        public int RowID { get; set; }
    }
}