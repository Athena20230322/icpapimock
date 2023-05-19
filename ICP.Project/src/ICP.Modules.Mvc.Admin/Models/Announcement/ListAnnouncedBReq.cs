using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Mvc.Admin.Models.Announcement
{
    /// <summary>
    /// 訊息公告清單Req
    /// </summary>
    public class ListAnnounceDbReq : PageModel
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }
    }
}
