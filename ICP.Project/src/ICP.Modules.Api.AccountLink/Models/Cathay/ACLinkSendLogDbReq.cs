namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 記錄傳送請求資料
    /// </summary>
    public class ACLinkSendLogDbReq
    {
        /// <summary>
        /// api類型
        /// </summary>
        public string ApiType { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 交易代號
        /// </summary>
        public string Msgid { get; set; }

        /// <summary>
        /// 交易來源
        /// </summary>
        public string Sourcechannel { get; set; }

        /// <summary>
        /// 業者交易序號
        /// </summary>
        public string Txnseq { get; set; }

        /// <summary>
        /// 發送訊息時間
        /// </summary>
        public string SendMsgTime { get; set; }

        /// <summary>
        /// 合作業者代號
        /// </summary>
        public string CooPerAtor { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public string MbrActNo { get; set; }

        /// <summary>
        /// 會員身份證字號(加密)
        /// </summary>
        public string MbrIdno { get; set; }

        /// <summary>
        /// 綁定結果通知
        /// </summary>
        public string ReplyApiURL { get; set; }

        /// <summary>
        /// 綁定後導回頁
        /// </summary>
        public string ReplyWebURL { get; set; }

        /// <summary>
        /// 銀行帳號(加密)
        /// </summary>
        public string BnkActNo { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 原訂單號碼
        /// </summary>
        public string Org_OrderNo { get; set; }

        /// <summary>
        /// 交易金額/退款金額/提領金額
        /// </summary>
        public int TxnAmt { get; set; }

        /// <summary>
        /// 欲查詢之交易序號
        /// </summary>
        public string QryTxnSeq { get; set; }

        /// <summary>
        /// 簽章值(PKCS7)
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// Hash(SHA256)
        /// </summary>
        public string DigestHash { get; set; }


    }
}
