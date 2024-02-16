using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    /// <summary>
    /// 更新會員同意項目
    /// </summary>
    public class SetMemberAgreeRequest: BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 同意項目編號
        /// </summary>
        public int AgreeType { get; set; }

        /// <summary>
        /// 同意狀態 0:尚未同意 1:同意 2:不同意
        /// </summary>
        public byte AgreeStatus { get; set; }
    }

    /// <summary>
    /// 更新會員同意項目列表
    /// </summary>
    public class SetMemberAgreeRequestItems
    {
        public SetMemberAgreeRequest[] AgreeItems { get; set; }
    }
}
