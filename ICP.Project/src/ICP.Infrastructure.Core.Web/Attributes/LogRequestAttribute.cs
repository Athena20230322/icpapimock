using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Abstractions;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Infrastructure.Core.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ICP.Infrastructure.Core.Web.Models.Consts;
using ICP.Infrastructure.Core.Web.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Frameworks.Logging;
using ICP.Infrastructure.Abstractions.Logging;
using Newtonsoft.Json.Linq;

namespace ICP.Infrastructure.Core.Web.Attributes
{
    public class LogRequestAttribute : ActionFilterAttribute
    {
        #region 公開屬性

        public Type Type { get; set; }

        public string Name { get; set; }

        public bool LogRequest { get; set; } = true;

        public bool LogTextResponse { get; set; }

        protected ILoggerFactory _loggerFactory = null;

        /// <summary>
        /// 遮罩 ex: new string[] { "request.Timestamp" }
        /// </summary>
        public object Masks { get; set; }   //微軟bug: 使用型態 string[] 會被 set 第 2 次為 string[0], 原因不明

        /// <summary>
        /// Autofac 相依注入
        /// </summary>
        public ILoggerFactory LoggerFactory
        {
            set
            {
                _loggerFactory = value;

                if (Type != null)
                {
                    Logger = value.CreateLogger(Type);
                }
                else if (!string.IsNullOrWhiteSpace(Name))
                {
                    Logger = value.CreateLogger(Name);
                }
                else
                {
                    Logger = value.CreateLogger<LogRequestAttribute>();
                }
            }
            get
            {
                return _loggerFactory;
            }
        }

        #endregion

        protected virtual ILogger Logger { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!LogRequest)
            {
                return;
            }

            var request = filterContext.HttpContext.Request;
            var args = filterContext.ActionParameters;
            var obj = new
            {
                RemoteRealIP = request.RemoteRealIP(),
                Headers = request.Headers?.ToDictionary(),
                Args = args
            };

            string json = null;
            var masks = Masks as string[];
            if (masks == null || masks.Length == 0 || args.Count == 0)
            {
                json = JsonConvert.SerializeObject(obj, CustomJsonSerializerSettings.IgnoreException);
            }
            else
            {
                var jObj = JObject.FromObject(obj);
                MaskProperties(jObj["Args"], masks);
                json = jObj.ToString();
            }

            Logger.Trace("[OnActionExecuting] {0}", json);
        }

        private void MaskProperties(JToken jToken, string[] Masks)
        {
            foreach (var mask in Masks)
            {
                try
                {
                    var token = jToken.SelectToken(mask, true);
                    if (token == null) continue;

                    var prop = token.Parent as JProperty;
                    if (prop == null) continue;

                    var value = (string)prop.Value;
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    value = string.Join(string.Empty, value.ToCharArray().Select(c => '*'));

                    prop.Value = value;
                }
                catch
                {
                    continue;
                }
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            if (!LogTextResponse)
            {
                return;
            }

            var response = filterContext.HttpContext.Response;
            string json = null;

            if (filterContext.Result is ContentResult contentResult)
            {
                json = JsonConvert.SerializeObject(new
                {
                    Headers = response.Headers?.ToDictionary(),
                    response.Status,
                    contentResult.ContentType,
                    contentResult.Content
                }, CustomJsonSerializerSettings.IgnoreException);
            }
            else if (filterContext.Result is JsonResult jsonResult)
            {
                json = JsonConvert.SerializeObject(new
                {
                    Headers = response.Headers?.ToDictionary(),
                    response.Status,
                    jsonResult.Data
                }, CustomJsonSerializerSettings.IgnoreException);
            }

            if (!string.IsNullOrWhiteSpace(json))
            {
                Logger.Trace("[OnResultExecuted] {0}", json);
            }
        }
    }
}
