using System.Web.Mvc;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Modules.Api.CheckEinvoiceToken.Commands;

namespace ICP.Modules.Api.CheckEinvoiceToken.Controllers
{
    
    public class CheckEinvoiceTokenController:Controller
    {
        private CheckEinvoiceTokenCommand _checkEinvoiceTokenCommand= null;

        public CheckEinvoiceTokenController(CheckEinvoiceTokenCommand checkEinvoiceTokenCommand)
        {
            _checkEinvoiceTokenCommand = checkEinvoiceTokenCommand;
        }

        [HttpPost]
        public string Index(InvoiceBindReturn model)
        {
            return _checkEinvoiceTokenCommand.checkIssueToken(model);
        }
    }
}