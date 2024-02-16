using System;

namespace ICP.Modules.Mvc.Admin.Models.Banner
{
    /// <summary>
    /// 新增廣告Req
    /// </summary>
    public class AddBannerReq
    {
        /// <summary>
        /// 廣告標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 廣告連結(第一層)
        /// </summary>
        public string UrlLink1 { get; set; }

        /// <summary>
        /// 圖片路徑(第一層)
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
        /// 建立者/修改人員
        /// </summary>
        public string Modifier { get; set; }

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
        /// 圖片路徑(第二層)
        /// </summary>
        public string ImagePath2 { get; set; }
    }

    /// <summary>
    /// 新增廣告位置Req
    /// </summary>
    public class AddBannerSiteReq
    {
        /// <summary>
        /// 廣告編號
        /// </summary>
        public int BannerID { get; set; }

        /// <summary>
        /// 廣告位置清單
        /// </summary>
        public string SiteIDList { get; set; }
    }

    /// <summary>
    /// 新增廣告內頁圖片Req
    /// </summary>
    public class AddImageReq
    {
        /// <summary>
        /// 廣告編號
        /// </summary>
        public int BannerID { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    /// 修改廣告Req
    /// </summary>
    public class EditBannerReq
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
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

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
        /// 建立者/修改人員
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 廣告內容
        /// </summary>
        public string BannerContent { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        /// <remarks>0:預設 1:審核通過 2: 退件</remarks>
        public int AuthStatus { get; set; }

        /// <summary>
        /// 審核者
        /// </summary>
        public string AuthUser { get; set; }

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
        /// 圖片路徑(第二層)
        /// </summary>
        public string ImagePath2 { get; set; }
    }
}
