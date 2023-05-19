using System.Web.Mvc;
using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Modules.Mvc.Member.Commands;

namespace ICP.Modules.Mvc.Member.Controllers
{
    public class EinvoiceBindController :BaseMvcController
    {
        private EinvocieBindCommand _einvocieBindCommand;
        private ILogger<EinvoiceBindController> _logger;
        private IAuthorizationFactory _authorizationFactory;
        private readonly IUserManager _userManager = null;

        public EinvoiceBindController(
            EinvocieBindCommand einvocieBindCommand, 
            ILogger<EinvoiceBindController> logger, 
            IAuthorizationFactory authorizationFactory)
        {
            _einvocieBindCommand = einvocieBindCommand;
            _logger = logger;
            _authorizationFactory = authorizationFactory;
            _userManager = authorizationFactory.Create(AuthorizationType.Mvc);
        }

        /// <summary>
        /// 電子發票載具綁定頁面
        /// </summary>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        public ActionResult InvoiceBind()
        {
            long mid;
            mid = _userManager.MID;

            InvoiceBindReturn RtnModel = new InvoiceBindReturn();

            if (mid != 0)
            {
                var getEInvoiceCarrierInfo = _einvocieBindCommand.GetEInvoiceCarrierInfo(mid);

                if (!string.IsNullOrWhiteSpace(getEInvoiceCarrierInfo.CarrierNumber))
                {
                    RtnModel = _einvocieBindCommand.InvoiceBindProcess(mid, getEInvoiceCarrierInfo.CarrierNumber);
                }
            }

            return View(RtnModel);
        }
    }
}