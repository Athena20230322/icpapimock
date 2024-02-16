namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// 第一銀行-連結綁定狀態查詢(ACLinkQuery) 回傳值
    /// </summary>
    public class ACLinkBindQryRes
    {
        /// <summary>
        /// 訊息序號
        /// </summary>
        public string MSG_NO { get; set; }

        /// <summary>
        /// 回應代碼
        /// </summary>
        public string RTN_CODE { get; set; }

        /// <summary>
        /// 回應訊息說明
        /// </summary>
        public string RTN_MSG { get; set; }

        /// <summary>
        /// 平台代碼
        /// </summary>
        public string EC_ID { get; set; }

        /// <summary>
        /// 平台會員代號
        /// </summary>
        public string EC_USER { get; set; }

        /// <summary>
        /// 帳號識別碼
        /// </summary>
        public string INDT_ACNT { get; set; }

        /// <summary>
        /// 綁定實體帳號
        /// </summary>
        public string LINK_ACNT { get; set; }

        /// <summary>
        /// 身份認證等級
        /// </summary>
        public string LINK_GRAD { get; set; }

        /// <summary>
        /// 訊息雜湊值
        /// </summary>
        public string RTN_DIGEST { get; set; }
    }
}
