using ICP.Library.Models.EinvoiceLibrary;
using ICP.Modules.Api.CheckEinvoiceToken.Services;

namespace ICP.Modules.Api.CheckEinvoiceToken.Commands
{
    public class CheckEinvoiceTokenCommand
    {
        private CheckEinvoiceTokenService _checkEinvoiceTokenService;

        public CheckEinvoiceTokenCommand(CheckEinvoiceTokenService checkEinvoiceTokenService)
        {
            _checkEinvoiceTokenService = checkEinvoiceTokenService;
        }

        public string checkIssueToken(InvoiceBindReturn model)
        {
           return _checkEinvoiceTokenService.checkIssueToken(model);
        }
    }
}