using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Mvc.Admin.Models.Announcement
{
    /// <summary>
    /// 訊息公告類別清單Req
    /// </summary>
    public class ListCategoryDbReq : PageModel
    {
        /// <summary>
        /// 類別編號
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 類別名稱
        /// </summary>
        public string CategoryName { get; set; }
    }
}
