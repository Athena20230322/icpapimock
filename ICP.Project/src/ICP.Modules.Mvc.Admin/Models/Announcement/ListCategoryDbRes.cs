using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Mvc.Admin.Models.Announcement
{
    /// <summary>
    /// 公告類別Res
    /// </summary>
    public class ListCategoryDbRes : BaseListModel
    {
        /// <summary>
        /// 公告類別ID
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 公告類別名稱
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        /// <remarks>0:未啟用 1: 啟用 2: 刪除</remarks>
        public int Status { get; set; }
    }
}
