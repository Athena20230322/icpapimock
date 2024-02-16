using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    /// <summary>
    /// 更新身份驗證資料
    /// </summary>
    public class UpdateMemberAuthIDNO
    {
        /// <summary>
        /// 身分證字號
        /// </summary>
        [Required]
        [RegularExpression(RegexConst.IDNO, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "身分證字號")]
        public string IDNO { get; set; }

        /// <summary>
        /// 發證日期
        /// </summary>
        [Required]
        [Display(Name = "領補換日期")]
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// 領取類別 (1:初發, 2:補發, 3:換發)
        /// </summary>
        [Required]
        [Range(1, 3)]
        [Display(Name = "領取類別")]
        public byte ObtainType { get; set; }

        /// <summary>
        /// 發證地點編號
        /// </summary>
        [Required]
        [Display(Name = "發證地點編號")]
        public string IssueLocationID { get; set; }

        /// <summary>
        /// 證上有無照片 (0: 無, 1:有)
        /// </summary>
        [Range(0, 1)]
        [Display(Name = "證上有無照片")]
        public byte IsPicture { get; set; }

        /// <summary>
        /// 證件正面照片
        /// </summary>
        [Display(Name = "證件正面照片")]
        public string FilePath1 { get; set; }

        /// <summary>
        /// 證件反面照片
        /// </summary>
        [Display(Name = "證件反面照片")]
        public string FilePath2 { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Display(Name = "備註")]
        public string AuthMsg { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Required]
        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }
    }
}
