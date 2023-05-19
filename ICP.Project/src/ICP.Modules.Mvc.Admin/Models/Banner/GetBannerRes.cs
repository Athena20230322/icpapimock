namespace ICP.Modules.Mvc.Admin.Models.Banner
{
    /// <summary>
    /// 取得廣告Res
    /// </summary>
    public class GetBannerRes
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
        public string UrlLink1 { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public string StartDateTime { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public string EndDateTime { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 是否有第二層內容
        /// </summary>
        /// <remarks>0否,1是</remarks>
        public int IsUseContent { get; set; }

        /// <summary>
        /// 頁面開啟設定(第一層)
        /// </summary>
        /// <remarks>0內開 1外開</remarks>
        public int OpenNewWindow1 { get; set; }

        /// <summary>
        /// 上架狀態
        /// </summary>
        /// <remarks>0下架,1上架,2刪除</remarks>
        public int Status { get; set; }

        /// <summary>
        /// 廣告內容
        /// </summary>
        public string BannerContent { get; set; }

        /// <summary>
        /// 廣告連結(第二層)
        /// </summary>
        public string UrlLink2 { get; set; }

        /// <summary>
        /// 頁面開啟設定(第二層)
        /// </summary>
        /// <remarks>0內開 1外開</remarks>
        public int? OpenNewWindow2 { get; set; }

        /// <summary>
        /// 內層BANNER圖片
        /// </summary>
        public string ImagePath2 { get; set; }
    }
}
