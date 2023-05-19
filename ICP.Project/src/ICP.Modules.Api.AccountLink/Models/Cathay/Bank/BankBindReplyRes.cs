namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-綁定結果通知(CubBindReply)
    /// </summary>
    public class BankBindReplyRes
    {
        /// <summary>
        /// 下行電文
        /// </summary>   
        /// <remarks>MSGID:ALSN001BINDING</remarks>
        public BankHeaderModel Header { get; set; }

    }
}
