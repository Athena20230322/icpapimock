using ICP.Infrastructure.Abstractions;
using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Exceptions;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Frameworks.EmailSender;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Infrastructure.Core.Web.Extensions;
using ICP.Infrastructure.Core.Web.Models;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ICP.Infrastructure.Core.Web.Attributes
{
    public class HandleErrorRequestAttribute : FilterAttribute, IExceptionFilter
    {
        private readonly GlobalAppSetting _globalAppSetting = null;
        private readonly ILogger _logger = null;

        public HandleErrorRequestAttribute(
            GlobalAppSetting globalAppSetting,
            ILogger<HandleErrorRequestAttribute> logger)
        {
            _globalAppSetting = globalAppSetting;
            _logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            //string errorType = "0";

            //if (filterContext.Exception is System.Data.SqlClient.SqlException)
            //{
            //    dbType = "1";
            //}

            //if (filterContext.Exception.ToString().IndexOf("SqlException") != -1 || filterContext.Exception.Message.IndexOf("SqlException") != -1)
            //{
            //    errorType = "1";
            //}

            //_logger.SetCustomVariables("errorType", errorType);
            _logger.Fatal(filterContext.Exception, "[OnException] {0}", filterContext.Exception.Message);

            //_logger.RemoveCustomVariables("errorType");

            filterContext.ExceptionHandled = true;
            filterContext.Result = errorResult(filterContext);

            return;
        }

        private ActionResult errorResult(ExceptionContext filterContext)
        {
            if (filterContext.Controller is BaseApiController)
            {
                var contentResult = new ContentResult
                {
                    ContentEncoding = Encoding.UTF8,
                    ContentType = MimeTypes.ApplicationJson
                };

                if (filterContext.Exception is BaseResultException baseResultException)
                {
                    contentResult.Content = baseResultException.Message;
                }
                else
                {
                    var apiResult = new ApiErrorResult
                    {
                        RequestId = _globalAppSetting.ProcessId
                    };

                    apiResult.SetFatalError();
                    contentResult.Content = JsonConvert.SerializeObject(apiResult);
                }

                return contentResult;
            }
            else
            {
                string host = filterContext.HttpContext.Request.Url.Host;
                string subDomain = host.Split('.')[0];
                string errorMsg = $"發生錯誤，請稍後再試{Environment.NewLine}{subDomain}:{_globalAppSetting.ProcessId}";

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var jsonResult = new JsonResult();
                    jsonResult.Data = new BaseResult { RtnMsg = errorMsg };
                    return jsonResult;
                }
                else
                {
                    var contentResult = new ContentResult();
                    contentResult.ContentEncoding = Encoding.UTF8;
                    contentResult.ContentType = MimeTypes.TextHtml;
                    contentResult.Content = errorMsg.Replace(Environment.NewLine, "<br/>");
                    return contentResult;
                }
            }
        }
    }
}
