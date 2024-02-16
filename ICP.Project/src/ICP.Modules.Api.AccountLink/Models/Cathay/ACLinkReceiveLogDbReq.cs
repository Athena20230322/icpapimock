namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 記錄傳送回應資料
    /// </summary>
    public class ACLinkReceiveLogDbReq
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
        /// 交易結果
        /// </summary>
        public string Returncode { get; set; }

        /// <summary>
        /// 交易結果說明
        /// </summary>
        public string Returndesc { get; set; }

        /// <summary>
        /// 業者交易序號
        /// </summary>
        public string Txnseq { get; set; }

        /// <summary>
        /// CUB交易序號
        /// </summary>
        public string FuseId { get; set; }

        /// <summary>
        /// 回覆訊息時間
        /// </summary>
        public string ReturnMsgTime { get; set; }

        /// <summary>
        /// 合作業者代號
        /// </summary>
        public string CooPerAtor { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public string MbrActNo { get; set; }

        /// <summary>
        /// CUB綁定作業網頁位址
        /// </summary>
        public string CubWebPage { get; set; }

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
        /// 會員身份證字號(加密)
        /// </summary>
        public string MbrIdno { get; set; }

        /// <summary>
        /// 綁定時間
        /// </summary>
        public string BindTime { get; set; }

        /// <summary>
        /// 目前狀態(01:申請 02:已綁定 03:取消綁定)
        /// </summary>
        public string ActStstus { get; set; }

        /// <summary>
        /// 銀行帳號(加密)
        /// </summary>
        public string BnkActNo { get; set; }

        /// <summary>
        /// 交易型態
        /// </summary>
        public string TransType { get; set; }

        /// <summary>
        /// 交易發生時間
        /// </summary>
        public string TxnDateTime { get; set; }

        /// <summary>
        /// 交易結果
        /// </summary>
        public string RtnCode { get; set; }

        /// <summary>
        /// 交易結果說明
        /// </summary>
        public string RtnDesc { get; set; }

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
