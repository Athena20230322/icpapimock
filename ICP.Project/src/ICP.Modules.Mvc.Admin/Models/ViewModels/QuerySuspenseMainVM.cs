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
    public class QuerySuspenseMainVM : PageModel
    {
        /// <summary>
        /// 起始日期
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string IDNO { get; set; }
    }
}
