using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ICP.Modules.Api.Authorization.FilterProxies
{
    using Infrastructure.Abstractions.Authorization;
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Models.Consts;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Web.Attributes;
    using Infrastructure.Core.Web.Frameworks.FilterProxy;
    using Library.Models.AuthorizationApi;
    using Commands;

    public class ApiAuthorizeFilterProxy : FilterProxy
    {
        private readonly IUserManager _userManager = null;
        private readonly IdentifyCommand _identifyCommand = null;
        private readonly GlobalAppSetting _globalAppSetting = null;

        private const string _actionParameterName = "request";

        public ApiAuthorizeFilterProxy(
            IdentifyCommand identifyCommand,
            IAuthorizationFactory authorizationFactory,
            GlobalAppSetting globalAppSetting)
        {
            _identifyCommand = identifyCommand;
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
            _globalAppSetting = globalAppSetting;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext, params object[] args)
        {
            base.OnActionExecuting(filterContext);

            var attributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
            bool allowAnonymous = (attributes != null && attributes.Length > 0);
            bool allowOPAnonymous = false;
            if (allowAnonymous)
            {
                attributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowOPAnonymousAttribute), true);
                if ((attributes != null && attributes.Length > 0)) allowOPAnonymous = true;
            }

            // 解密 + 驗證
            var processRequestResult = _identifyCommand.ProcessRequest(filterContext.HttpContext.Request, allowAnonymous, allowOPAnonymous);
            if (!processRequestResult.IsSuccess)
            {
                filterContext.Result = getResult(processRequestResult);
                return;
            }

            var context = processRequestResult.RtnData;
            _userManager.Login(new Dictionary<string, object>
            {
                { UserDataType.AuthorizationApiKeyContext, context.KeyContext },
                { UserDataType.EncryptData, context.EncryptData },
                { UserDataType.DecryptData, context.DecryptData },
                { UserDataType.MID, context.KeyContext.ClientAesCert.MID },
                { UserDataType.OPMID, context.OPMID },
                { UserDataType.AppTokenID, context.AppTokenID },
            });

            // 還原參數
            if (filterContext.ActionParameters.Keys.Contains(_actionParameterName))
            {
                Type targetType = filterContext.ActionParameters[_actionParameterName].GetType();

                var parseRequestModel = _identifyCommand.ParseRequestModel(targetType, context.DecryptData);
                if (!parseRequestModel.IsSuccess)
                {
                    filterContext.Result = getResult(parseRequestModel);
                    return;
                }

                filterContext.ActionParameters[_actionParameterName] = parseRequestModel.RtnData;
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext, params object[] args)
        {
            base.OnResultExecuting(filterContext);

            var response = filterContext.RequestContext.HttpContext.Response;
            response.Headers.Add("X-iCP-RequestId", _globalAppSetting?.ProcessId);

            if (!(filterContext.Result is JsonResult jsonResult) ||
                !(jsonResult.Data is DataResult dataResult))
            {
                return;
            }

            if (dataResult.RtnData == null)
            {
                dataResult.RtnData = new BaseAuthorizationApiResult();
            }

            var keyContext = _userManager.GetData<AuthorizationApiKeyContext>(UserDataType.AuthorizationApiKeyContext);

            var encryptApiResult = _identifyCommand.ProcessResponse(keyContext, dataResult);
            if (!encryptApiResult.IsSuccess)
            {
                jsonResult.Data = encryptApiResult.ToBaseResult();
                return;
            }

            // 簽章
            response.Headers.Add("X-iCP-Signature", encryptApiResult.RtnData.Signature);

            jsonResult.Data = encryptApiResult.RtnData.Result;
        }

        private ContentResult getResult(BaseResult result)
        {
            return new ContentResult
            {
                ContentEncoding = Encoding.UTF8,
                ContentType = MimeTypes.ApplicationJson,
                Content = JsonConvert.SerializeObject(new BaseResult
                {
                    RtnCode = result.RtnCode,
                    RtnMsg = result.RtnMsg
                }),
            };
        }
    }
}
