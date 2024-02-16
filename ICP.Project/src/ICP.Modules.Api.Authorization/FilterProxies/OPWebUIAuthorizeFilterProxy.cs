using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Infrastructure.Core.Web.Frameworks.FilterProxy;
using ICP.Library.Models.AuthorizationApi;
using ICP.Library.Models.OpenWalletApi.WebUIApi;
using ICP.Modules.Api.Authorization.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.Authorization.FilterProxies
{
    public class OPWebUIAuthorizeFilterProxy : FilterProxy
    {
        private const string _actionParameterName = "request";

        private readonly IUserManager _userManager = null;
        private readonly OPWebUIAuthorizeCommand _oPWebUIAuthorizeCommand = null;

        public OPWebUIAuthorizeFilterProxy(
            IAuthorizationFactory authorizationFactory,
            OPWebUIAuthorizeCommand oPWebUIAuthorizeCommand
            )
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
            _oPWebUIAuthorizeCommand = oPWebUIAuthorizeCommand;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext, params object[] args)
        {
            base.OnActionExecuting(filterContext);

            var attributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
            bool allowAnonymous = (attributes != null && attributes.Length > 0);

            Type targetType = null;

            string MethodName = filterContext.ActionDescriptor.ActionName;

            // 參數類型
            if (filterContext.ActionParameters.Keys.Contains(_actionParameterName))
            {
                targetType = filterContext.ActionParameters[_actionParameterName].GetType();
            }

            // 解密 + 驗證
            long MID = 0;
            string decryptData = null;
            var processRequestResult = _oPWebUIAuthorizeCommand.ProcessRequest(filterContext.HttpContext.Request, allowAnonymous, MethodName, targetType, ref decryptData, ref MID);
            if (!processRequestResult.IsSuccess)
            {
                filterContext.Result = getResult(processRequestResult);
                return;
            }

            var context = processRequestResult.RtnData;
            _userManager.Login(new Dictionary<string, object>
            {
                { UserDataType.MID, MID }
            });

            // 還原參數
            if (targetType != null)
            {
                var parseRequestModel = _oPWebUIAuthorizeCommand.ParseRequestModel(targetType, decryptData);
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
                !(jsonResult.Data is BaseWebUIApiResult rtnData)
                )
            {
                return;
            }

            var encryptApiResult = _oPWebUIAuthorizeCommand.ProcessResponse(rtnData);
            if (!encryptApiResult.IsSuccess)
            {
                jsonResult.Data = getResult(encryptApiResult).Data;
                return;
            }

            jsonResult.Data = encryptApiResult.RtnData;
            base.OnResultExecuting(filterContext, args);
        }

        private JsonResult getResult(BaseResult result)
        {            
            return new JsonResult
            {
                Data = new BaseWebUIApiResult
                {
                    StatusCode = result.IsSuccess ? "0000" : "0001",
                    StatusMessage = result.RtnMsg
                }
            };
        }
    }
}
