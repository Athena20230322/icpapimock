using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetAppXmlSettingRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// XML版號
        /// </summary>
        public string XmlVersion { get; set; }
    }
}
