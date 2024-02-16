using ICP.Infrastructure.Core.Models.Consts;
using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class AuthIDNORequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [Display(Name = "姓名")]
        public string CName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Required]
        [Display(Name = "生日")]
        public string BirthDay { get; set; }

        /// <summary>
        /// 國籍編號
        /// </summary>
        [Required]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "國籍編號")]
        public string NationalityID { get; set; }

        /// <summary>
        /// 縣市+鄉鎮市區
        /// </summary>
        [Required]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "縣市鄉鎮市區")]
        public string AreaID { get; set; }

        /// <summary>
        /// 地址(巷弄路段號)
        /// </summary>
        [Required(ErrorMessage = "請輸入正確的地址資料")]
        [Display(Name = "縣市鄉鎮市區")]
        public string Address { get; set; }

        /// <summary>
        /// 電子郵件帳號
        /// </summary>
        [RegularExpression(RegexConst.Email, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "電子郵件帳號")]
        public string Email { get; set; }

        /// <summary>
        /// 身份證字號
        /// </summary>
        [Required]
        [RegularExpression(RegexConst.IDNO, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "身分證字號")]
        public string Idno { get; set; }

        /// <summary>
        /// 發證日期，格式：yyyy-MM-dd
        /// </summary>
        [Required]
        [Display(Name = "發證日期")]
        public string IssueDate { get; set; }

        /// <summary>
        /// 發證地點 (ex.10017)
        /// </summary>
        [Required]
        [Display(Name = "發證地點")]
        public string IssueLoc { get; set; }

        /// <summary>
        /// 換證類別
        /// 1 = 初發
        /// 2 = 補發
        /// 3 = 換發
        /// </summary>
        [Required]
        [RegularExpression("^[1-3]+$", ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "換證類別")]
        public string IssueType { get; set; }

        /// <summary>
        /// 法定代理人資料
        /// </summary>
        public List<LegalRepData> LegalRepData { get; set; }

        /// <summary>
        /// 身分證正反面
        /// </summary>
        public HttpPostedFileBase[] Files { get; set; }
    }
}
