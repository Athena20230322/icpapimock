using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Modules.Api.AccountLink.Controllers;
using ICP.Modules.Api.AccountLink.Services;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Web.Mvc;

namespace ICP.Modules.Api.AccountLink.Attributes
{
    /// <summary>
    /// 驗證API來源參數資料
    /// </summary>
    public class ACLinkValidateAttribute : ActionFilterAttribute
    {
        public ACLinkValidateService ValidateService { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller is BaseAccountLinkController)
            {
                ((BaseAccountLinkController)filterContext.Controller).Injection(ValidateService);
            }

            string token = Convert.ToString(filterContext.RequestContext.HttpContext.Request["token"]);

            // 解密 + 驗證
            var processRequestResult = ValidateService.ProcessRequest(token);

            if (!processRequestResult.IsSuccess)
            {
                filterContext.Result = GetResult(processRequestResult);
                return;
            }

            if (filterContext.Controller is BaseAccountLinkController iacLink)
            {
                iacLink.ACKey = processRequestResult.RtnData.ACKey;
                iacLink.ACIV = processRequestResult.RtnData.ACIV;
                iacLink.ACModel = processRequestResult.RtnData.ACModel;
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Controller is BaseAccountLinkController)
            {
                ((BaseAccountLinkController)filterContext.Controller).Injection(ValidateService);
            }

            base.OnResultExecuting(filterContext);

            var response = filterContext.RequestContext.HttpContext.Response;
            
            if (!(filterContext.Result is JsonResult jsonResult))
            {
                return;
            }

            if (!(jsonResult.Data is DataResult dataResult))
            {
                return;
            }

            var encryptApiResult = ValidateService.ProcessResponse(dataResult);
            if (!encryptApiResult.IsSuccess)
            {
                jsonResult.Data = GetResult(encryptApiResult);
                return;
            }

            jsonResult.Data = encryptApiResult.RtnData.ACModel.Json;
        }

        private ContentResult GetResult(BaseResult result)
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
