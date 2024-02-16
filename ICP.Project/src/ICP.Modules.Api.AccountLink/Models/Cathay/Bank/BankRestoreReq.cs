using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-提領(CubRestore)
    /// </summary>
    public class BankRestoreReq
    {
        /// <summary>
        /// 上行電文
        /// </summary>   
        /// <remarks>MSGID:ALSC002RESTORE</remarks>
        [Required(ErrorMessage = "({0})")]
        public BankHeaderModel header { get; set; }

        /// <summary>
        /// 發送訊息時間
        /// </summary>
        [Required(ErrorMessage = "發送訊息時間({0})")]
        public string sendMsgTime { get; set; }

        /// <summary>
        /// 合作業者代號
        /// </summary>
        /// <remarks>銀行端編定提供</remarks>
        [Required(ErrorMessage = "合作業者代號({0})")]
        public string cooPerAtor { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        [Required(ErrorMessage = "會員編號({0})")]
        public string mbrActNo { get; set; }

        /// <summary>
        /// 會員身份證字號
        /// </summary>
        /// <remarks>需加密</remarks>
        [Required(ErrorMessage = "會員身份證字號({0})")]
        public string mbrIdno { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        /// <remarks>需加密</remarks>
        [Required(ErrorMessage = "銀行帳號({0})")]
        public string bnkActNo { get; set; }

        /// <summary>
        /// 提領金額
        /// </summary>
        [Required(ErrorMessage = "提領金額({0})")]
        public int txnAmt { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        [Required(ErrorMessage = "訂單號碼({0})")]
        public string orderNo { get; set; }

        /// <summary>
        /// 簽章值(PKCS7)
        /// </summary>
        /// <remarks>交易序號+合作業者代號+會員編號+銀行帳號(加密前)+訂單號碼+提領金額+發送訊息時間</remarks>
        [Required(ErrorMessage = "簽章({0})")]
        public string sign { get; set; }

    }
}
