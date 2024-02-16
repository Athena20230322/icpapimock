using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class AddOverSeaMemberDataModel : ValidatableObject
    {
        /// <summary>
        /// 手機號碼
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.CellPhone, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }

        /// <summary>
        /// 會員姓名
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.ChineseCName, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "姓名")]
        public string CName { get; set; }

        /// <summary>
        /// 國籍代碼
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "國籍代碼")]
        public long NationalID { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "郵遞區號")]
        public string ZipCode { get; set; }

        /// <summary>
        /// 區域代碼
        /// </summary>
        public string AreaID { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]      
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// OPMID
        /// </summary>       
        [Required(ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "OPMID")]
        public string OPMID { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.Email, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }

        /// <summary>
        /// 居留證字號
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.UniformID, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "居留證字號")]
        public string UniformID { get; set; }

        /// <summary>
        /// 發證日期
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]        
        //[RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "發證日期")]
        public DateTime UniformIssueDate { get; set; }

        /// <summary>
        /// 居留證到期日
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        //[RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "居留證到期日")]
        public DateTime UniformExpireDate { get; set; }

        /// <summary>
        /// 背面流水號
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "居留證流水號")]
        public string UniformNumber { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "銀行代碼")]
        public string BankCode { get; set; }

        /// <summary>
        /// 分行代碼
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "分行代碼")]
        public string BankBranchCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "銀行帳號")]
        public string BankAccount { get; set; }
        
        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 使用者密碼
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }
        
        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }
                        
    }
}
