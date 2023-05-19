using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-綁定結果通知(CubBindReply)(銀行->業者)
    /// </summary>
    public class BankBindReplyReq
    {
        /// <summary>
        /// 上行電文
        /// </summary>   
        /// <remarks>MSGID:ALSN001BINDING</remarks>
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
        /// Hash(SHA256)
        /// </summary>
        /// <remarks>交易序號+會員編號+銀行帳號(加密前)+發送訊息時間</remarks>
        [Required(ErrorMessage = "加密({0})")]
        public string digestHash { get; set; }

    }
}
