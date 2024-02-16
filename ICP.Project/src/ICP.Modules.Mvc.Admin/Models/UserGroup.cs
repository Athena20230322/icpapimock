using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class UserGroup
    {
        /// <summary>
        /// 群組ID
        /// </summary>
        public int UserGroupID { get; set; }

        /// <summary>
        /// 群組編號
        /// </summary>
        [Required]
        [Display(Name = "群組編號")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "請輸入2-10個字元，可中英混合")]
        public string UserGroupCode { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        [Required]
        [Display(Name = "群組名稱")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "請輸入2-40個字元，可中英混合")]
        public string UserGroupName { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Display(Name = "備註")]
        [StringLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 顯示
        /// </summary>
        public byte Visible { get; set; }
    }
}
