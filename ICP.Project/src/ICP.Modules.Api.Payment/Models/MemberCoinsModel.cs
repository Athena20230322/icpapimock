namespace ICP.Modules.Api.Payment.Models
{
    public class MemberCoinsModel
    {
        /// <summary>
        /// 會員代碼
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 現金帳戶金額
        /// </summary>
        public decimal RealCash { get; set; }

        /// <summary>
        /// 儲值帳戶金額
        /// </summary>
        public decimal TopUpCash { get; set; }

        /// <summary>
        /// 凍結款
        /// </summary>
        public decimal FreezeCash { get; set; }
    }
}
