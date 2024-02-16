using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Infrastructure.Core.Web.Models;
using ICP.Library.Models.AuthorizationApi;
using ICP.Modules.Api.Member.Commands;
using ICP.Modules.Api.Member.Models;
using ICP.Modules.Api.Member.Models.Certificate;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.Member.Controllers
{
    public class CertificateController : BaseApiController
    {
        private readonly CertificateCommand _certificateCommand = null;
        private readonly IUserManager _userManager = null;
        private readonly IAuthorizationFactory _authorizationFactory = null;

        public CertificateController(
            CertificateCommand certificateCommand,
            IAuthorizationFactory authorizationFactory)
        {
            _certificateCommand = certificateCommand;
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
            _authorizationFactory = authorizationFactory;
        }

        [HttpPost]
        public ActionResult GetDefaultPucCert()
        {
            var result = _certificateCommand.GetDefaultPucCert();
            if (!result.IsSuccess)
            {
                return Json(result.ToBaseResult());
            }

            return Json(new
            {
                result.RtnCode,
                result.RtnMsg,
                DefaultPubCertID = result.RtnData.CertId,
                DefaultPubCert = result.RtnData.PublicCert
            });
        }

        [HttpPost]
        public ActionResult ExchangePucCert(string encData)
        {
            long pubCertID = long.TryParse(Request.Headers["X-iCP-DefaultPubCertID"], out pubCertID) ? pubCertID : 0;
            string signature = Request.Headers["X-iCP-Signature"];

            var result = _certificateCommand.ExchangePucCert(pubCertID, signature, encData);

            long clientCertId = (result.RtnData?.ServerPubCertID).GetValueOrDefault();
            return apiResult(clientCertId, result);
        }

        [HttpPost]
        public ActionResult GenerateAES(string encData)
        {
            long clientCertId = long.TryParse(Request.Headers["X-iCP-ServerPubCertID"], out clientCertId) ? clientCertId : 0;
            string signature = Request.Headers["X-iCP-Signature"];

            var result = _certificateCommand.GenerateAes(clientCertId, signature, encData);

            return apiResult(clientCertId, result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult BindMerchantCert(BindMerchantCertRequest request)
        {
            var keyContext = _userManager.GetData<AuthorizationApiKeyContext>(UserDataType.AuthorizationApiKeyContext);
            long clientCertId = keyContext.ClientAesCert.ClientCertId;

            var result = _certificateCommand.BindMerchantCert(clientCertId, request);

            return AppResult(result);
        }

        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        public ActionResult TestApi(BaseAuthorizationApiRequest request)
        {
            var result = new DataResult<BaseAuthorizationApiResult>();
            result.SetSuccess(new BaseAuthorizationApiResult());

            return AppResult(result);
        }

        public ActionResult AppRedirect(AppRedirectRequest request)
        {
            var result = _certificateCommand.AppRedirect(request);
            if (!result.IsSuccess)
            {
                return Content(result.RtnMsg);
            }

            var Expired = DateTime.Now.AddDays(1);  // cookie 期限
            IUserManager mvcUserManager = _authorizationFactory.Create(AuthorizationType.Mvc);
            mvcUserManager.Login(new Dictionary<string, object>
                {
                    { Library.Models.AuthorizationMvc.UserDataType.MID, result.RtnData.MID },
                    { Library.Models.AuthorizationMvc.UserDataType.IsWebView, true }
                }, Expired);

            if (string.IsNullOrWhiteSpace(result.RtnData.Url))
            {
                return Content(string.Empty);
            }

            return Redirect(result.RtnData.Url);
        }

        private ActionResult apiResult<T>(long clientCertId, DataResult<T> dataResult)
        {
            if (clientCertId == 0 || !dataResult.IsSuccess)
            {
                return Json(dataResult.ToBaseResult());
            }

            var result = _certificateCommand.EncryptApiResult(clientCertId, dataResult, dataResult.RtnData);
            if (!result.IsSuccess)
            {
                return Json(result.ToBaseResult());
            }

            Response.Headers.Add("X-iCP-Signature", result.RtnData.Signature);

            return Json(result.RtnData.Result);
        }
    }
}
