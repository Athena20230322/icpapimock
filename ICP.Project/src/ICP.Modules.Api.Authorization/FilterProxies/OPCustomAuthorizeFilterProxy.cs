using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ICP.Modules.Api.Authorization.FilterProxies
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Models.Consts;
    using Infrastructure.Core.Web.Frameworks.FilterProxy;
    using Library.Models.OpenWalletApi.CustomReceiveApi;
    using Commands;

    public class OPCustomAuthorizeFilterProxy : FilterProxy
    {
        private readonly GlobalAppSetting _globalAppSetting = null;
        private readonly OPCustomAuthorizeCommand _opApiAuthorizeCommand = null;

        private const string _actionParameterName = "request";

        Dictionary<string, object> _keyContext = null;

        public OPCustomAuthorizeFilterProxy(
            GlobalAppSetting globalAppSetting,
            OPCustomAuthorizeCommand oPApiAuthorizeCommand
            )
        {
            _globalAppSetting = globalAppSetting;
            _opApiAuthorizeCommand = oPApiAuthorizeCommand;
            _keyContext = new Dictionary<string, object>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext, params object[] args)
        {
            base.OnActionExecuting(filterContext);

            Type targetType = null;

            string MethodName = filterContext.ActionDescriptor.ActionName;

            // 參數類型
            if (filterContext.ActionParameters.Keys.Contains(_actionParameterName))
            {
                targetType = filterContext.ActionParameters[_actionParameterName].GetType();
            }

            // 解密 + 驗證
            string decryptData = null;
            var processRequestResult = _opApiAuthorizeCommand.ProcessRequest(filterContext.HttpContext.Request, MethodName, targetType, ref decryptData);
            if (!processRequestResult.IsSuccess)
            {
                filterContext.Result = getResult(processRequestResult);
                return;
            }

            var context = processRequestResult.RtnData;

            // 還原參數
            if (targetType != null)
            {
                var parseRequestModel = _opApiAuthorizeCommand.ParseRequestModel(targetType, decryptData);
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
            if (!(filterContext.Result is JsonResult jsonResult) ||
                !(jsonResult.Data is BaseCustomReceiveApiResult rtnData)
                )
            {
                return;
            }

            var encryptApiResult = _opApiAuthorizeCommand.ProcessResponse(rtnData: rtnData);
            if (!encryptApiResult.IsSuccess)
            {
                filterContext.Result = getResult(string.Empty);
                return;
            }

            filterContext.Result = getResult(encryptApiResult.RtnData);
            return;
        }

        private ContentResult getResult(BaseResult result)
        {
            var encryptApiResult = _opApiAuthorizeCommand.ProcessResponse(baseResult: result);
            if (!encryptApiResult.IsSuccess)
            {
                return getResult(string.Empty);
            }

            return getResult(encryptApiResult.RtnData);
        }

        private ContentResult getResult(string content)
        {
            return new ContentResult
            {
                ContentEncoding = Encoding.UTF8,
                ContentType = MimeTypes.ApplicationJson,
                Content = content
            };
        }
    }
}
