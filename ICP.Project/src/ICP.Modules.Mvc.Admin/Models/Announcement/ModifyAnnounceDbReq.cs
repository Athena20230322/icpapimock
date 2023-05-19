using System;

namespace ICP.Modules.Mvc.Admin.Models.Announcement
{
    #region 訊息公告內容
    /// <summary>
    /// 新增公告Req
    /// </summary>
    public class AddContentDbReq
    {
        /// <summary>
        /// 公告標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 訊息類別
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 公告上架日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 公告下架日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 頂置狀況
        /// </summary>
        /// <remarks>0: 預設值, 1: 頂置</remarks>
        public byte IsTop { get; set; }

        /// <summary>
        /// 置頂起始時間
        /// </summary>
        public DateTime? IsTopStartDate { get; set; }

        /// <summary>
        /// 置頂結束時間
        /// </summary>
        public DateTime? IsTopEndDate { get; set; }

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
        /// 建立者/修改人員
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 公告內容
        /// </summary>
        public string AnnounceContent { get; set; }
    }

    /// <summary>
    /// 新增圖片Req
    /// </summary>
    public class AddImageDbReq
    {
        /// <summary>
        /// 公告編號
        /// </summary>
        public int NID { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    /// 新增MID名單檔案Req
    /// </summary>
    public class AddMidFileDbReq
    {
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

        /// <summary>
        /// 建立者/修改人員
        /// </summary>
        public string Modifier { get; set; }
    }

    /// <summary>
    /// 新增MID名單Req
    /// </summary>
    public class AddMidDbReq
    {
        /// <summary>
        /// 檔案編號
        /// </summary>
        public int FileID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }
    }

    /// <summary>
    /// 修改公告Req
    /// </summary>
    public class EditContentDbReq
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
        /// 公告上架日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 公告下架日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 頂置狀況
        /// </summary>
        /// <remarks>0: 預設值, 1: 頂置</remarks>
        public byte IsTop { get; set; }

        /// <summary>
        /// 置頂起始時間
        /// </summary>
        public DateTime? IsTopStartDate { get; set; }

        /// <summary>
        /// 置頂結束時間
        /// </summary>
        public DateTime? IsTopEndDate { get; set; }

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
        /// 建立者/修改人員
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 公告內容
        /// </summary>
        public string AnnounceContent { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        /// <remarks>0:預設 1:審核通過 2: 退件</remarks>
        public int AuthStatus { get; set; }

        /// <summary>
        /// 審核者
        /// </summary>
        public string AuthUser { get; set; }
    }

    /// <summary>
    /// 修改MID名單Req
    /// </summary>
    public class EditMidDbReq
    {
        /// <summary>
        /// 檔案編號
        /// </summary>
        public int FileID { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        /// <remarks>0: 刪除 1: 正常</remarks>
        public int Status { get; set; }

        /// <summary>
        /// 修改人員
        /// </summary>
        public string Modifier { get; set; }
    }
    #endregion

    #region 訊息公告類別
    /// <summary>
    /// 新增公告類別Req
    /// </summary>
    public class AddCategoryDbReq
    {
        /// <summary>
        /// 公告類別名稱
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 公告類別狀態
        /// </summary>
        /// <remarks>0:未啟用 1: 啟用 2: 刪除</remarks>
        public int Status { get; set; }

        /// <summary>
        /// 建立者/修改人員
        /// </summary>
        public string Modifier { get; set; }
    }

    /// <summary>
    /// 修改公告類別Req
    /// </summary>
    public class EditCategoryDbReq
    {
        /// <summary>
        /// 類別編號
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 公告類別名稱
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 公告類別狀態
        /// </summary>
        /// <remarks>0:未啟用 1: 啟用 2: 刪除</remarks>
        public int Status { get; set; }

        /// <summary>
        /// 建立者/修改人員
        /// </summary>
        public string Modifier { get; set; }
    }
    #endregion

    #region Add Log
    /// <summary>
    /// AddContentLog Req
    /// </summary>
    public class AddContentLogDbReq
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
        /// 公告上架日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 公告下架日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 頂置狀況
        /// </summary>
        /// <remarks>0: 預設值, 1: 頂置</remarks>
        public byte IsTop { get; set; }

        /// <summary>
        /// 置頂起始時間
        /// </summary>
        public DateTime? IsTopStartDate { get; set; }

        /// <summary>
        /// 置頂結束時間
        /// </summary>
        public DateTime? IsTopEndDate { get; set; }

        /// <summary>
        /// 排序編號
        /// </summary>
        public int OrderID { get; set; }

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
        /// 審核狀態
        /// </summary>
        /// <remarks>0:預設 1:審核通過 2: 退件</remarks>
        public int AuthStatus { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? AuthDate { get; set; }

        /// <summary>
        /// 審核人員
        /// </summary>
        public string AuthUser { get; set; }

        /// <summary>
        /// 建立者/修改人員
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }
    }

    /// <summary>
    /// AddCategoryLog Req
    /// </summary>
    public class AddCategoryLogDbReq
    {
        /// <summary>
        /// 類別編號
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 公告類別名稱
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 公告類別狀態
        /// </summary>
        /// <remarks>0:未啟用 1: 啟用 2: 刪除</remarks>
        public int Status { get; set; }

        /// <summary>
        /// 建立者/修改人員
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }
    }
    #endregion
}
