using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.Banner
{
    public class QryBannerVM: PageModel
    {
        /// <summary>
        /// 上架狀態
        /// </summary>
        /// <remarks>1待審核、2審核通過、3審核不通過、4審核通過上架中、5審核通過已下架</remarks>
        [Display(Name = "狀態")]
        public int BannerStatus { get; set; }

        /// <summary>
        /// 上架狀態選單
        /// </summary>
        /// <remarks>1待審核、2審核通過、3審核不通過、4審核通過上架中、5審核通過已下架</remarks>
        [Display(Name = "狀態")]
        public IEnumerable<SelectListItem> BannerStatusList { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        [Display(Name = "廣告起迄時間")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        [Display(Name = "廣告起迄時間")]
        public DateTime? EndDate { get; set; }
    }
}
