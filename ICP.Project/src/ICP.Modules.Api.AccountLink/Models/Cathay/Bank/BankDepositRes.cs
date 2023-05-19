namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-儲值(CubDeposit)
    /// </summary>
    public class BankDepositRes
    {
        /// <summary>
        /// 下行電文
        /// </summary>   
        /// <remarks>MSGID:ALSD002DEPOSIT</remarks>
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
        /// 交易金額
        /// </summary>
        public int TxnAmt { get; set; }

        /// <summary>
        /// Hash(SHA256)
        /// </summary>
        /// <remarks>交易序號+合作業者代號+會員編號+交易金額+回覆訊息時間</remarks>
        public string DigestHash { get; set; }

    }
}
