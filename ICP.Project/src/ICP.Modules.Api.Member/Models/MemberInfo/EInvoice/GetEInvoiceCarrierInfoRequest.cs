using ICP.Library.Models.AuthorizationApi;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    /// <summary>
    /// 驗證手機條碼 Request 
    /// </summary>
    public class GetEInvoiceCarrierInfoRequest :BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 手機條碼載具號碼
        /// </summary>
        public string CarrierNumber { get; set; }
        /// <summary>
        /// 手機條碼載具驗證碼。如參數NULL則用上次驗證成功的驗證碼來進行驗證
        /// </summary>
        public string VerificationCode { get; set; }
        
    }
}