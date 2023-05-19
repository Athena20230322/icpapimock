using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    using ICP.Infrastructure.Core.Models;
    using Infrastructure.Core.ValidationAttributes;

    public class UpdateTeenagerModel: ValidatableObject
    {
        /// <summary>
        /// 未成年身份證資料
        /// </summary>
        [ValidateObjectAttribute]
        [Display(Name = "未成年身份證資料")]
        public UpdateMemberAuthIDNO AuthIDNO { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(60)]
        [Display(Name = "姓名")]
        public string CName { get; set; }

        /// <summary>
        /// 審核備註
        /// </summary>
        [StringLength(200)]
        [Display(Name = "審核備註")]
        public string Note { get; set; }
    }
}
