using ICP.Infrastructure.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.TopUp
{
    public class AutoTopUpConditionReq : ValidatableObject
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long MID { get; set; }

        /// <summary>
        /// 自動儲值模式 (1：差額儲值, 2：定額儲值)
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, 2, ErrorMessage = "{0} 格式不正確")]
        public int TopUpMode { get; set; }

        /// <summary>
        /// 單次儲值上限
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, 50000, ErrorMessage = "{0} 格式不正確")]
        public int LimitAmount { get; set; }

        /// <summary>
        /// 單日儲值上限
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, 50000, ErrorMessage = "{0} 格式不正確")]
        public int DailyLimitAmount { get; set; }

        /// <summary>
        /// 定額儲值金額基數
        /// </summary>
        [Range(1, 50000, ErrorMessage = "{0} 格式不正確")]
        public int TopUpUnit { get; set; }

        /// <summary>
        /// 銀行帳戶編號(系統定義流水號)
        /// </summary>
        [Range(0, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long AccountID { get; set; }

        /// <summary>
        /// 自動儲值開關 (0：關閉, 1：開啟)
        /// </summary>
        [Range(0, 1, ErrorMessage = "{0} 格式不正確")]
        public int TopUpSwitch { get; set; }
    }
}
