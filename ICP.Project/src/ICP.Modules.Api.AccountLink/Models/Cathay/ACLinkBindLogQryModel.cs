namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 綁定申請記錄-查詢條件
    /// </summary>
    public class ACLinkBindLogQryModel
    {
        /// <summary>
        /// api類型
        /// </summary>
        public string ApiType { get; set; }

        /// <summary>
        /// 業者交易序號
        /// </summary>
        public string Txnseq { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public string MbrActNo { get; set; }
    }
}
