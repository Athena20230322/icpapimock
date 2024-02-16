using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.Member.Controllers
{
    using Infrastructure.Abstractions.FilterProxy;
    using Infrastructure.Core.Web.Attributes;
    using Infrastructure.Core.Web.Controllers;
    using Library.Models.OpenWalletApi.CustomReceiveApi;
    using Commands;
    using ICP.Infrastructure.Core.Models;

    [ActionFilterProxy(ProxyType = ProxyType.OPCustomApi)]
    [LogRequest(LogTextResponse = true)]
    public class OPCustomController: BaseApiController
    {
        OPCustomCommand _oPCustomCommand;

        protected override ActionResult AppResult(DataResult obj)
        {
            var rtnData = (BaseCustomReceiveApiResult)obj.RtnData;

            if (string.IsNullOrWhiteSpace(rtnData.Code))
            {
                if (obj.IsSuccess)
                {
                    rtnData.Code = "00";
                }
                else
                {
                    rtnData.Code = obj.RtnCode.ToString().PadLeft(2, '0');
                }
            }

            return Json(rtnData);
        }

        public OPCustomController(OPCustomCommand oPCustomCommand)
        {
            _oPCustomCommand = oPCustomCommand;
        }

        [HttpPost]
        public ActionResult NoticeMemberDelete(NoticeMemberDeleteRequest request)
        {
            var result = _oPCustomCommand.NoticeMemberDelete(request);

            return AppResult(result);
        }

        [HttpPost]
        public ActionResult NoticeMobileBarcode(NoticeMobileBarcodeRequest request)
        {
            var result = _oPCustomCommand.NoticeMobileBarcode(request);

            return AppResult(result);
        }
    }
}
