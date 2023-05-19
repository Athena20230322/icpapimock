namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// 第一銀行-連結帳戶綁定(ACLinkBind) 回傳值1
    /// </summary>
    public class ACLinkBindRes1
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
        /// Session Key
        /// </summary>
        public string S_KEY { get; set; }

        /// <summary>
        /// 連結綁定URL
        /// </summary>
        public string LINK_URL { get; set; }

        /// <summary>
        /// 訊息雜湊值
        /// </summary>
        public string RTN_DIGEST { get; set; }
    }
}
