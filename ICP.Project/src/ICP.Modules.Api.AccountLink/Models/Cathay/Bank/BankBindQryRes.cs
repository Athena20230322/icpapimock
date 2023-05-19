using System.Collections.Generic;

namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-綁定查詢(CubBindQry)
    /// </summary>
    public class BankBindQryRes
    {
        /// <summary>
        /// 下行電文
        /// </summary>   
        /// <remarks>MSGID:ALSQ001BINDING</remarks>
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
        /// 會員身份證字號
        /// </summary>
        /// <remarks>需加密</remarks>
        public string MbrIdno { get; set; }

        /// <summary>
        /// Hash(SHA256)
        /// </summary>
        /// <remarks>交易序號+合作業者代號+會員編號+回覆訊息時間</remarks>
        public string DigestHash { get; set; }

        /// <summary>
        /// 綁定記錄
        /// </summary>
        /// <remarks>只記錄最新一筆</remarks>
        public List<BankRecordInfoModel> RecordInfo { get; set; }
    }
}
