using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.Banner
{
    /// <summary>
    /// 廣告清單Req
    /// </summary>
    public class ListBannerReq : PageModel
    {
        /// <summary>
        /// 上架狀態
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public int? AuthStatus { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
