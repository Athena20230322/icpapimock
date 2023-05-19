using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    public class UpdateMemberAuthUniformID
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 居流證號碼
        /// </summary>
        [RegularExpression(RegexConst.UniformID, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "居留證號碼")]
        public string UniformID { get; set; }

        /// <summary>
        /// 居留證核發日期
        /// </summary>
        [Display(Name = "居留證核發日期")]
        public DateTime? UniformIssueDate { get; set; }

        /// <summary>
        /// 居留期限
        /// </summary>
        [Display(Name = "居留期限")]
        public DateTime? UniformExpireDate { get; set; }

        /// <summary>
        /// 居留證流水號
        /// </summary>
        [Display(Name = "居留證流水號")]
        public string UniformNumber { get; set; }

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
        /// 生日
        /// </summary>
        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }
    }
}
