namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-綁定(CubAlsBind)
    /// </summary>
    public class BankBindRes
    {
        /// <summary>
        /// 下行電文
        /// </summary>   
        /// <remarks>MSGID:ALSM001BINDING</remarks>
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
        /// Hash(SHA256)
        /// </summary>
        /// <remarks>交易序號+合作業者代號+會員編號+回覆訊息時間</remarks>
        public string DigestHash { get; set; }

        /// <summary>
        /// CUB綁定作業網頁位址
        /// </summary>
        /// <remarks>?accessToken=xx&cooPerAtor=x</remarks>
        public string CubWebPage { get; set; }

    }
}
