using ICP.Library.Models.AuthorizationApi;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class ListBankBranchRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        [Required]
        public string BankCode { get; set; }
    }
}
