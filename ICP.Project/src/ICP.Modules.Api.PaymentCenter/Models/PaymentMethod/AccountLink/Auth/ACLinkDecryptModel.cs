using ICP.Library.Models.AccountLinkApi.Enums;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink.Auth
{
    public class ACLinkDecryptModel
    {
        public string Json { get; set; }

        public BankType BankType { get; set; }
    }
}
