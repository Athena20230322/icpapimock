using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class P11AuthVM
    {
        /// <summary>
        /// 身份證字號
        /// </summary>
        [Required]
        [Display(Name = "身份證字號")]
        public string IDNO { get; set; }

        /// <summary>
        /// 發證日期(年)
        /// </summary>
        [Required]
        [Display(Name = "領補換日期(年)")]
        public int IssueDateYear { get; set; }
        /// <summary>
        /// 發證日期(月)
        /// </summary>
        [Required]
        [Display(Name = "領補換日期(月)")]
        public int IssueDateMonth { get; set; }
        /// <summary>
        /// 發證日期(日)
        /// </summary>
        [Required]
        [Display(Name = "領補換日期(日)")]
        public int IssueDateDay { get; set; }
        /// <summary>
        /// 領取類別 (1:初發, 2:補發, 3:換發)
        /// 預設初發
        /// </summary>
        public byte ObtainType { get; set; } = 1;
        /// <summary>
        /// 發證地點編號
        /// </summary>
        public string IssueLocationID { get; set; }
        /// <summary>
        /// 證上有無照片 (0: 無, 1:有)
        /// 預設印
        /// </summary>
        public byte IsPicture { get; set; } = 1;
        /// <summary>
        /// 生日日期(年)
        /// </summary>
        [Required]
        [Display(Name = "出生日期(年)")]
        public int BirthdayYear { get; set; }
        /// <summary>
        /// 生日日期(月)
        /// </summary>
        [Required]
        [Display(Name = "出生日期(月)")]
        public int BirthdayMonth { get; set; }
        /// <summary>
        /// 生日日期(日)
        /// </summary>
        [Required]
        [Display(Name = "出生日期(日)")]
        public int BirthdayDay { get; set; }
    }
}
