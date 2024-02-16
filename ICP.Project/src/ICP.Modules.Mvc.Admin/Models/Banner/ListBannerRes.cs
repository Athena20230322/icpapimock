using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.Banner
{
    /// <summary>
    /// 廣告清單Res
    /// </summary>
    public class ListBannerRes : BaseListModel
    {
        /// <summary>
        /// 廣告編號
        /// </summary>
        public int BannerID { get; set; }

        /// <summary>
        /// 廣告標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 廣告連結(第一層)
        /// </summary>
        public string UrlLink { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 上架狀態
        /// </summary>
        /// <remarks>0下架,1上架,2刪除</remarks>
        public int Status { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        /// <remarks>0:預設 1:審核通過 2: 退件</remarks>
        public int AuthStatus { get; set; }

        /// <summary>
        /// 最後修改日期
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 最後修改者
        /// </summary>
        public string Modifier { get; set; }
    }
}
