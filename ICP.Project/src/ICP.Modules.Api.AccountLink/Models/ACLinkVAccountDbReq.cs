namespace ICP.Modules.Api.AccountLink.Models
{
    public class ACLinkVAccountDbReq
    {
        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Payment的訂單流水號
        /// </summary>
        public long TradeID { get; set; }
    }
}
