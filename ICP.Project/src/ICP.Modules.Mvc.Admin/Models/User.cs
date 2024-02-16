using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ICP.Infrastructure.Core.Models.Consts;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class User
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        [Required]
        [Display(Name = "請輸入P+8碼員工編號")]
        [RegularExpression(@"^[P]{1}[0-9]{8}$", ErrorMessage = "請輸入P+8碼員工編號")]
        public string Account { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Required]
        [Display(Name = "使用者名稱")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "請輸入2-40字元員工真實名稱")]
        public string CName { get; set; }

        /// <summary>
        /// 部門編號
        /// </summary>
        [Required(ErrorMessage = "請選擇員工所在部門")]
        [Display(Name = "部門編號")]
        public int? DeptID { get; set; }

        /// <summary>
        /// 是否為主管
        /// </summary>
        [Range(0, 1)]
        [Display(Name = "是否為主管")]
        public byte IsManager { get; set; }

        /// <summary>
        /// 使用者Email
        /// </summary>
        [Required]
        [Display(Name = "使用者Email")]
        [StringLength(100)]
        [RegularExpression(RegexConst.Email, ErrorMessage = "請輸入正確格式的E-mail")]
        public string UserEmail { get; set; }

        /// <summary>
        /// 使用者狀態( 0: Disable, 1:Active)
        /// </summary>
        [Range(0, 1)]
        [Display(Name = "使用者狀態")]
        public byte UserStatus { get; set; }

        /// <summary>
        /// 員工編號
        /// </summary>
        [Display(Name = "員工編號")]
        [StringLength(20)]
        public string EID { get; set; }

        /// <summary>
        /// 建檔時間
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
