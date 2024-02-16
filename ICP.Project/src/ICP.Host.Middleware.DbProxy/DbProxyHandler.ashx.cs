using Autofac;
using Autofac.Integration.Web.Forms;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Infrastructure.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Web;

namespace ICP.Host.Middleware.DbProxy
{
    /// <summary>
    /// DbProxyHandler 的摘要描述
    /// </summary>
    [InjectProperties]
    public class DbProxyHandler : IHttpHandler
    {
        public bool IsReusable => false;

        /// <summary>
        /// 相依注入
        /// </summary>
        public IComponentContext ComponentContext { get; set; }

        private readonly AesCryptoHelper _aesCryptoHelper = null;

        public DbProxyHandler()
        {
            _aesCryptoHelper = new AesCryptoHelper
            {
                Key = GlobalConfigUtil.DbProxy_HashKey,
                Iv = GlobalConfigUtil.DbProxy_HashIv
            };
        }

        public void ProcessRequest(HttpContext context)
        {
            var result = new DataResult<string>();
            result.SetError();

            try
            {
                var processRequestResult = processRequest(context.Request);
                if (!processRequestResult.IsSuccess)
                {
                    result.SetError(processRequestResult);
                    return;
                }

                var processInvokeResult = processInvoke(processRequestResult.RtnData);
                if (!processInvokeResult.IsSuccess)
                {
                    result.SetError(processInvokeResult);
                    return;
                }

                string json = JsonConvert.SerializeObject(processInvokeResult.RtnData);
                result.SetSuccess(json);
            }
            catch (Exception ex)
            {
                result.SetError();
                result.RtnData = ex.ToString();
                throw ex;
            }
            finally
            {
                processResponse(context.Response, result);
            }
        }

        private DataResult<DbProxyRequest> processRequest(HttpRequest httpRequest)
        {
            var result = new DataResult<DbProxyRequest>();
            result.SetError();

            string encryptData = httpRequest.Form[nameof(DbProxyRequest)];
            if (string.IsNullOrWhiteSpace(encryptData))
            {
                result.SetCode(10002, nameof(encryptData));
                return result;
            }

            string decryptData = _aesCryptoHelper.Decrypt(encryptData);
            if (string.IsNullOrWhiteSpace(decryptData))
            {
                result.SetCode(10003, nameof(encryptData));
                return result;
            }

            var dbProxyRequest = JsonConvert.DeserializeObject<DbProxyRequest>(decryptData);
            if (dbProxyRequest == null)
            {
                result.SetCode(10004, nameof(encryptData));
                return result;
            }

            result.SetSuccess(dbProxyRequest);
            return result;
        }

        private DataResult<object> processInvoke(DbProxyRequest dbProxyRequest)
        {
            var result = new DataResult<object>();
            result.SetError();

            string typeName = $"{dbProxyRequest.FullName}, {dbProxyRequest.AssemblyName}";
            Type type = Type.GetType(typeName);
            if (type == null || !ComponentContext.TryResolve(type, out object obj))
            {
                result.SetCode(10005, typeName);
                return result;
            }

            var method = obj.GetType().GetMethod(dbProxyRequest.MethodName);
            if (method == null)
            {
                result.SetCode(10006, typeName);
                return result;
            }

            var methodParameters = method.GetParameters();
            object[] methodArgs = methodParameters.Select((x, idx) =>
            {
                var jToken = JToken.FromObject(dbProxyRequest.Args[idx]);
                return jToken.ToObject(x.ParameterType);
            }).ToArray();

            var methodResult = method.Invoke(obj, methodArgs);

            result.SetSuccess(methodResult);
            return result;
        }

        private void processResponse(HttpResponse httpResponse, object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            string encryptData = _aesCryptoHelper.Encrypt(json);

            httpResponse.ContentType = MimeTypes.ApplicationJson;
            httpResponse.Write(encryptData);
        }
    }
}