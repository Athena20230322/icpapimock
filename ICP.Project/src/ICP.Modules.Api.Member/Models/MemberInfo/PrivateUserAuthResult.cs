using System.Collections.Generic;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using ICP.Library.Models.MemberModels;
    using Library.Models.AuthorizationApi;

    public class PrivateUserAuthResult : BaseAuthorizationApiResult
    {
        public string TimeStamp { get; set; }

        /// <summary>
        /// OPEN WALLET會員帳號
        /// </summary>
        public string OpwMID { get; set; }

        /// <summary>
        /// icashpay電支帳號
        /// </summary>
        public string IcpMID { get; set; }
    }
}