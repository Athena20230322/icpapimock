namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class TradeDbReq
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 轉帳類型(1:轉入, 0:轉出)
        /// </summary>
        public int ReceiveTransfer { get; set; }
    }
}