namespace ICP.Modules.Mvc.Admin.Models.Announcement
{
    /// <summary>
    /// 取得公告Res
    /// </summary>
    public class GetContentDbRes
    {
        /// <summary>
        /// 公告編號
        /// </summary>
        public int NID { get; set; }

        /// <summary>
        /// 公告標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 訊息類別
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 訊息類別名稱
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 公告上架日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 公告上架時間
        /// </summary>
        public string StartDateTime { get; set; }

        /// <summary>
        /// 頂置狀況
        /// </summary>
        /// <remarks>0: 預設值, 1: 頂置</remarks>
        public int IsTop { get; set; }

        /// <summary>
        /// 置頂起始日期
        /// </summary>
        public string IsTopStartDate { get; set; }

        /// <summary>
        /// 置頂起始時間
        /// </summary>
        public string IsTopStartDateTime { get; set; }

        /// <summary>
        /// 置頂結束日期
        /// </summary>
        public string IsTopEndDate { get; set; }

        /// <summary>
        /// 置頂結束時間
        /// </summary>
        public string IsTopEndDateTime { get; set; }

        /// <summary>
        /// 公告狀況
        /// </summary>
        /// <remarks>0: 下架,1: 上架, 2: 刪除</remarks>
        public byte AnnounceStatus { get; set; }

        /// <summary>
        /// 發送對象
        /// </summary>
        /// <remarks>0: 全會員 1:自行上傳名單</remarks>
        public byte AnnounceType { get; set; }

        /// <summary>
        /// 公告內容
        /// </summary>
        public string AnnounceContent { get; set; }
    }

    /// <summary>
    /// 取得MID名單檔案Res
    /// </summary>
    public class GetMidFileDbRes
    {
        /// <summary>
        /// 檔案編號
        /// </summary>
        public int FileID { get; set; }

        /// <summary>
        /// 公告編號
        /// </summary>
        public int NID { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        /// <remarks>0: 刪除 1: 正常</remarks>
        public int Status { get; set; }
    }
}