namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// 第一銀行-取消連結帳戶綁定(ACLinkCancel) 回傳值
    /// </summary>
    public class ACLinkCancelRes
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
        /// 訊息雜湊值
        /// </summary>
        public string RTN_DIGEST { get; set; }
    }
}
