using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Library.Models.AuthorizationApi;
using ICP.Modules.Api.Payment.Models.Transfer;
using System.Web.Mvc;

namespace ICP.Modules.Api.Payment.Controllers
{
    public class TransferController : BaseApiController
    {
        private readonly IUserManager _userManager = null;

        public TransferController
        (
            IAuthorizationFactory authorizationFactory
        )
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
        }

        /// <summary>
        /// 取得轉帳Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult GetTransferToken(GetTransferTokenReq request)
        {           
            long mid = _userManager.GetData<long>(UserDataType.MID); //### 若沒有加入Filter:ActionFilterProxy,此方式無效

            DataResult<GetTransferTokenRes> result = new DataResult<GetTransferTokenRes>();

            return AppResult(result);
        }        
    }
}
