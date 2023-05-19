using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class QueryMemberResultVM : BaseListModel
    {
        /// <summary>
        /// 手機號碼
        /// </summary>
        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }

        /// <summary>
        /// 登入帳號
        /// </summary>
        [Display(Name = "登入帳號")]
        public string Account { get; set; } 

        /// <summary>
        /// 身份證字號
        /// </summary>
        [Display(Name = "身份證字號")]
        public string IDNO { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        [Display(Name = "電支帳號")]
        public string ICPMID { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        [Display(Name = "統一編號")]
        public string UnifiedBusinessNo { get; set; }                          

        /// <summary>
        /// 公司名稱
        /// </summary>
        [Display(Name = "公司名稱")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 統一證號/居留證號
        /// </summary>
        [Display(Name = "居留證字號")]
        public string UniformID { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        [Display(Name = "商店名稱")]
        public string WebSiteName { get; set; }

        /// <summary>
        /// 會員姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string CName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// 電支帳號狀態
        /// </summary>
        [Display(Name = "電支帳號狀態")]
        public string StatusName { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        [Display(Name = "MID")]
        public long MID { get; set; }
    }
}
