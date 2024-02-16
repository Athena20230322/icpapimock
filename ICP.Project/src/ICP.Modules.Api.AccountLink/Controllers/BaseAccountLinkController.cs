using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Library.Models.AccountLinkApi;
using ICP.Modules.Api.AccountLink.Services;

namespace ICP.Modules.Api.AccountLink.Controllers
{
    public abstract class BaseAccountLinkController : BaseApiController
    {
        public string ACKey { get; set; }

        public string ACIV { get; set; }

        public ACLinkDecryptModel ACModel { get; set; }

        public ACLinkValidateService ValidateService { get; set; }

        public void Injection(ACLinkValidateService validateService)
        {
            ValidateService = validateService;
        }
    }
}
