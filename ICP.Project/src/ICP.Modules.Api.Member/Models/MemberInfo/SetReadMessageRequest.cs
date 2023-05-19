using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class SetReadMessageRequest : BaseAuthorizationApiRequest
    {
        public class Param
        {
            public long MsgID { get; set; }
        }

        /// <summary>
        /// 是否已讀全部訊息
        /// False：否(預設值) 
        /// True：是
        /// </summary>
        public bool ReadAll { get; set; }

        /// <summary>
        /// 額外參數 
        /// ※當ReadAll=False時，此欄位必填
        /// </summary>
        public List<Param> Params { get; set; }
    }
}
