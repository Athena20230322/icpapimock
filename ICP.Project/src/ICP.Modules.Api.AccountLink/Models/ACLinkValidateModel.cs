using ICP.Library.Models.AccountLinkApi;

namespace ICP.Modules.Api.AccountLink.Models
{
    /// <summary>
    /// 驗證
    /// </summary>
    public class ACLinkValidateModel
    {
        public string ACKey { get; set; }

        public string ACIV { get; set; }

        public ACLinkDecryptModel ACModel { get; set; }
    }
}
