using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class LegalRepData
    {
        /// <summary>
        /// 法定代理人類別
        /// 1 = 父母
        /// 2 = 非父母
        /// 3 = 單一監護人
        /// </summary>
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0} 格式錯誤")]
        public string LegalRepType { get; set; }

        /// <summary>
        /// 法定代理人姓名
        /// </summary>
        public string LegalRepName { get; set; }

        /// <summary>
        /// 法定代理人icash Pay電支帳號
        /// </summary>
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0} 格式錯誤")]
        public string LegalRepIcpMID { get; set; }
    }
}
