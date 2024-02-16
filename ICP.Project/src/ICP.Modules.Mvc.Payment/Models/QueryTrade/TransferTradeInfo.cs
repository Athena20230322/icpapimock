namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class TransferTradeInfo : TradeInfo
    {
        /// <summary>
        /// 轉帳/接受者姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 轉帳備註
        /// </summary>
        public string Remark { get; set; }
    }
}