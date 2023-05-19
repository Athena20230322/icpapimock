using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class AddFunctionSwitchRevVM : ValidatableObject
    {
        /// <summary>
        /// 預設開關資料
        /// </summary>
        [ValidateObjectAttribute]
        public FunctionCategoryStatusRev FunctionCategoryStatusRev { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        [Required(ErrorMessage = "請選擇開關預設開始時間")]
        public string SwitchStartDate { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        [Required(ErrorMessage = "請選擇開關預設開始時間")]
        public string SwitchStartTime { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        [Required(ErrorMessage = "請選擇開關預設關閉時間，關閉時間到了後，功能狀態將依照主開關狀態變動")]
        public string SwitchEndDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        [Required(ErrorMessage = "請選擇開關預設關閉時間，關閉時間到了後，功能狀態將依照主開關狀態變動")]
        public string SwitchEndTime { get; set; }
    }
}
