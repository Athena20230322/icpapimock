using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class AddAppFunctionSwitchRevVM : ValidatableObject
    {
        /// <summary>
        /// 預設開關資料
        /// </summary>
        [ValidateObjectAttribute]
        public AppFunctionSwitchRev AppFunctionSwitchRev { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        [Required]
        public string SwitchStartDate { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        [Required]
        public string SwitchStartTime { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        [Required]
        public string SwitchEndDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        [Required]
        public string SwitchEndTime { get; set; }
    }
}
