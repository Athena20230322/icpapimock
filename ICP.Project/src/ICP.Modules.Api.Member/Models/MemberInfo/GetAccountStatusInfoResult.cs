using ICP.Library.Models.AuthorizationApi;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetAccountStatusInfoResult : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 是否有未結束收帳的收帳訂單
        /// </summary>
        public bool RequestTransferStatus { get; set; }

        /// <summary>
        /// 是否有餘額
        /// </summary>
        public bool AccountStatus { get; set; }

        /// <summary>
        /// 是否有提領中的餘額
        /// </summary>
        public bool WithdrawStatus { get; set; }

        /// <summary>
        /// 是否有待撥款的款項
        /// </summary>
        public bool AllocateStatus { get; set; }
    }
}
