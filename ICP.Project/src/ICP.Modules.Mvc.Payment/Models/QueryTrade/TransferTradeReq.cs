namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class TransferTradeReq : TradeReq
    {
        /// <summary>
        /// 轉帳類型(1:轉入, 0:轉出)
        /// </summary>
        public int ReceiveTransfer { get; set; }
    }
}