using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    /// <summary>
    /// APP 設定
    /// </summary>
    public class APPSetting: ValidatableObject
    {
        /// <summary>
        /// XML版本號
        /// </summary>
        [Required]
        [Display(Name = "XML版本號")]
        public byte VersionNo { get; set; }

        /// <summary>
        /// 正式的XML內容
        /// </summary>
        [System.Web.Mvc.AllowHtml]
        [Display(Name = "正式的XML內容")]
        public string XMLData { get; set; }

        /// <summary>
        /// 測試用XML內容
        /// </summary>
        [Required]
        [System.Web.Mvc.AllowHtml]
        [Display(Name = "測試用XML內容")]
        public string TestXMLData { get; set; }

        /// <summary>
        /// 更新說明
        /// </summary>
        [Required]
        [StringLength(200)]
        [Display(Name = "更新說明")]
        public string ReleaseNote { get; set; }

        /// <summary>
        /// 測試MID
        /// </summary>
        [Display(Name = "測試MID")]
        public string TestMID { get; set; }

        /// <summary>
        /// 上線時間
        /// </summary>
        public DateTime? ReleaseTime { get; set; }
    }
}
