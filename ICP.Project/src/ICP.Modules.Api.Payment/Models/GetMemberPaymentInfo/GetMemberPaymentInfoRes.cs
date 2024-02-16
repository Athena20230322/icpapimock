using ICP.Library.Models.AuthorizationApi;
using System.Collections.Generic;

namespace ICP.Modules.Api.Payment.Models.GetMemberPaymentInfo
{
    public class GetMemberPaymentInfoRespose : BaseAuthorizationApiResult
    {
        public List<AccountLinkRes> AcctLinkList { get; set; }

        public iCashAccountInfo IcashpayList { get; set; }
    }
}
