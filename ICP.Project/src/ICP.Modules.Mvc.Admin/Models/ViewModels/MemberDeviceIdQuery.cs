using System;
using System.ComponentModel.DataAnnotations;
using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    /// <summary>
    /// 裝置ID管理 Query
    /// </summary>
    public class MemberDeviceIdQuery: PageModel
    {
        /// <summary>
        /// 查詢日期 啟始
        /// </summary>
        [Required]
        [Display(Name = "起始日期")]
        public DateTime? CreateDateBegin { get; set; }

        /// <summary>
        /// 查詢日期 結束
        /// </summary>
        [Required]
        [Display(Name = "結束日期")]
        public DateTime CreateDateEnd { get; set; }

        /// <summary>
        /// 鎖定狀態(0:解鎖, 1:封鎖)，預設值=1
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// App DeviceID
        /// </summary>
        [Display(Name = "請輸入裝置ID")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "請輸入2-50字元裝置ID")]
        public string DeviceID { get; set; }
    }
}