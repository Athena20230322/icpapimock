using ICP.Library.Models.AuthorizationApi;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetWithdrawBalanceInfoResult : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行分行代碼
        /// </summary>
        public string BankBranchCode { get; set; }

        /// <summary>
        /// 帳號末五碼
        /// </summary>
        public string AccountLast5No { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 分行名稱
        /// </summary>
        public string BankBranchName { get; set; }

        /// <summary>
        /// 提領手續費
        /// </summary>
        public int HandlingCharge { get; set; }

        /// <summary>
        /// 可提領金額
        /// </summary>
        public int AvailableWithdrawCash { get; set; }
    }
}
