using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-綁定查詢(CubBindQry)
    /// </summary>
    public class BankBindQryReq
    {
        /// <summary>
        /// 上行電文
        /// </summary>   
        /// <remarks>MSGID:ALSQ001BINDING</remarks>
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
        /// 簽章值(PKCS7)
        /// </summary>
        /// <remarks>交易序號+合作業者代號+會員編號+會員身份證字號(加密前)+發送訊息時間</remarks>
        [Required(ErrorMessage = "簽章({0})")]
        public string sign { get; set; }

    }
}
