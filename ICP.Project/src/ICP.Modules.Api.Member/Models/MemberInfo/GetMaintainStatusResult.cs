using ICP.Library.Models.AuthorizationApi;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetMaintainStatusResult : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 系統功能ID
        /// </summary>
        public string ItemID { get; set; }

        /// <summary>
        /// 系統功能名稱
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 狀態
        /// 1 = 開啟
        /// 2 = 關閉
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 狀態說明
        /// </summary>
        public string Description { get; set; }
    }
}
