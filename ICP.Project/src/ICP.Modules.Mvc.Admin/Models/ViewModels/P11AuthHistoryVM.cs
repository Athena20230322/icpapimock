using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class P11AuthHistoryVM : PageModel
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
        /// 身分證號
        /// </summary>
        public string IDNO { get; set; }
        /// <summary>
        /// 是否通過驗證
        /// 0:驗證失敗
        /// 1:驗證成功
        /// null:全選
        /// </summary>
        public short? IsPass { get; set; }
    }
}
