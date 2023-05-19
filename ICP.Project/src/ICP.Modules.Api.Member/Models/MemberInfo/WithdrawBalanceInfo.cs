namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class WithdrawBalanceInfo
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 提領類別
        /// 0 = 提領轉入帳戶
        /// 1 = 連結扣款帳戶
        /// </summary>
        public byte Category { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行分行代碼
        /// </summary>
        public string BankBranchCode { get; set; }

        /// <summary>
        /// 帳號帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 分行名稱
        /// </summary>
        public string BankBranchName { get; set; }
    }
}
