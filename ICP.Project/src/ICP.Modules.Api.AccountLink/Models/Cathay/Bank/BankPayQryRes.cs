using System.Collections.Generic;

namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-綁定查詢(CubTxnQry)
    /// </summary>
    public class BankPayQryRes
    {
        /// <summary>
        /// 下行電文
        /// </summary>   
        /// <remarks>MSGID:ALSQ002TRANSACTION</remarks>
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
        /// 銀行帳號
        /// </summary>
        /// <remarks>需加密</remarks>
        public string BnkActNo { get; set; }

        /// <summary>
        /// 交易型態
        /// </summary>
        /// <remarks>ALSD001PAYMENT:付款 ALSD002DEPOSIT:儲值 ALSC001REFUND:退款 ALSC002RESTORE:提領 </remarks>
        public string TransType { get; set; }

        /// <summary>
        /// 交易發生時間
        /// </summary>
        public string TxnDateTime { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int TxnAmt { get; set; }

        /// <summary>
        /// 交易結果
        /// </summary>
        public string RtnCode { get; set; }

        /// <summary>
        /// 交易結果說明
        /// </summary>
        public string RtnDesc { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// Hash(SHA256)
        /// </summary>
        /// <remarks>交易序號+合作業者代號+會員編號+回覆訊息時間</remarks>
        public string DigestHash { get; set; }

    }
}
