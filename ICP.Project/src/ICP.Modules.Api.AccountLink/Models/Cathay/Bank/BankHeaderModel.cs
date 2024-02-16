using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-上/下行電文
    /// </summary>
    public class BankHeaderModel
    {
        /// <summary>
        /// 交易代號
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        public string msgid { get; set; }

        /// <summary>
        /// 交易來源
        /// </summary>
        public string sourcechannel { get; set; }

        /// <summary>
        /// 回覆值/交易結果
        /// </summary>
        /// <remarks>Request不帶值/成功:0000</remarks>
        public string returncode { get; set; }

        /// <summary>
        /// 回覆說明/交易結果說明
        /// </summary>
        /// <remarks>Request不帶值</remarks>
        public string returndesc { get; set; }

        /// <summary>
        /// 業者交易序號
        /// </summary>
        /// <remarks>業者代碼(4)+交易日期(yyyymmdd)+序號(8)</remarks>
        [Required(ErrorMessage = "{0} is required.")]
        public string txnseq { get; set; }

        /// <summary>
        /// CUB交易序號
        /// </summary>
        /// <remarks>Request不帶值</remarks>
        public string fuseID { get; set; }

    }
}
