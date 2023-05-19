namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// 第一銀行-平台交易結果查詢(ACLinkPayQuery) 接收參數
    /// </summary>
    public class ACLinkPayQryModel : BaseACLinkModel
    {
        /// <summary>
        /// 查詢訊息序號
        /// </summary>
        public string SerMsgNo { get; set; }
    }
}
