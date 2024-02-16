using ICP.Infrastructure.Core.Models.Consts;
using ICP.Library.Models.AuthorizationApi;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetMaintainStatusRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 系統功能ID
        /// </summary>
        [RegularExpression(RegexConst.Only_Number)]
        public string ItemID { get; set; }
    }
}
