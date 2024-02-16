using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class P33AuthHistoryVM : PageModel
    {
        /// <summary>
        /// 電支編號
        /// </summary>
        public string ICPMID { get; set; }
        /// <summary>
        /// 驗證日期(起)
        /// </summary>
        [Required]
        [Display(Name = "起始日期")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 驗證日期(迄)
        /// </summary>
        [Required]
        [Display(Name = "結束日期")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// ID的類別
        /// 0 : 身分證字號
        /// 1 : 統編
        /// </summary>
        public byte IDTypes { get; set; } = 0;
        /// <summary>
        /// 身份證字號
        /// </summary>
        [Display(Name = "身份證字號")]
        public string IDNO { get; set; }
        /// <summary>
        /// 統一編號
        /// </summary>
        [Display(Name = "統一編號")]
        public string UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 是否通過驗證
        /// 0:驗證失敗
        /// 1:驗證成功
        /// null:全選
        /// </summary>
        public short? IsPass { get; set; }
    }
}
