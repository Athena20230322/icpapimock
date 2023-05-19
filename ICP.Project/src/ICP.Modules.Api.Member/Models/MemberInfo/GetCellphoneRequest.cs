using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using Library.Models.AuthorizationApi;

    public class GetCellphoneRequest: BaseAuthorizationApiRequest
    {
        /// <summary>
        /// OPEN WALLET 提供的AuthV
        /// </summary>
        [Required]
        public string AuthV { get; set; }

        /// <summary>
        /// 登入帳號(For 測試工具登入用)
        /// </summary>
        public string UserCode { get; set; }
    }
}
