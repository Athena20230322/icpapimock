using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 會員偏好設定
    /// </summary>
    public class MemberPreferenceModel
    {
        /// <summary>
        /// 設定名稱
        /// </summary>
        [Required]
        public string OptionName { get; set; }

        /// <summary>
        /// 設定值
        /// </summary>
        public string OptionValue { get; set; }
    }
}
