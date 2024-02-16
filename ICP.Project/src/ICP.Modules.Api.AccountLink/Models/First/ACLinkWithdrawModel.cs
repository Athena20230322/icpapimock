namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// 第一銀行-連結帳戶交易提領(ACLinkWithdraw) 接收參數
    /// </summary>
    public class ACLinkWithdrawModel : BaseACLinkModel
    {
        /// <summary>
        /// 提領交易時間
        /// </summary>
        public string TradeTime { get; set; } = System.DateTime.Now.ToString("yyyyMMddHHmmss");

        /// <summary>
        /// 提領交易明細
        /// </summary>
        public string TradeNote { get; set; } = "ICash提領";

        /// <summary>
        /// 提領金額
        /// </summary>
        public int Amount { get; set; }
    }
}
