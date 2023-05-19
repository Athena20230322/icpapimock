namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-退款(CubRefund)
    /// </summary>
    public class BankRefundRes
    {
        /// <summary>
        /// 下行電文
        /// </summary>   
        /// <remarks>MSGID:ALSC001REFUND</remarks>
        public BankHeaderModel Header { get; set; }

        /// <summary>
        /// 回覆訊息時間
        /// </summary>
        public string ReturnMsgTime { get; set; }

        /// <summary>
        /// 合作業者代號
        /// </summary>
        /// <remarks>銀行端編定提供</remarks>
        public string CooPerAtor { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public string MbrActNo { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 原訂單號碼
        /// </summary>
        public string Org_OrderNo { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public int TxnAmt { get; set; }

        /// <summary>
        /// Hash(SHA256)
        /// </summary>
        /// <remarks>交易序號+合作業者代號+會員編號+退款金額+回覆訊息時間</remarks>
        public string DigestHash { get; set; }

    }
}
