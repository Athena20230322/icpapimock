using Castle.DynamicProxy;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Exceptions;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.AOP
{
    public class DbProxyInterceptor : IInterceptor
    {
        private readonly ILoggerFactory _loggerFactory = null;

        public DbProxyInterceptor(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void Intercept(IInvocation invocation)
        {
            var attributes = invocation.Method.GetCustomAttributes(typeof(EnableDbProxyAttribute), true);
            var attribute = attributes?.FirstOrDefault();
            ILogger logger = _loggerFactory.CreateLogger(invocation.TargetType);

            if (GlobalConfigUtil.DbProxy_Enable &&
                attribute != null &&
                attribute is EnableDbProxyAttribute enableDbProxyAttribute)
            {
                logger.Trace($"方法：{invocation.Method.Name} 符合攔截條件，準備開始執行 {nameof(DbProxyInterceptor)}");
                logger.Trace($"執行 processPreRequest...");
                var requestDict = processPreRequest(invocation);

                logger.Trace($"執行 processRequest...");
                string response = processRequest(requestDict, enableDbProxyAttribute.TransferTimeoutSec);

                logger.Trace($"執行 processResult...");
                var resultObj = processResult(response, invocation.Method.ReturnType);

                invocation.ReturnValue = resultObj;
            }
            else
            {
                invocation.Proceed();
            }
        }

        private IDictionary<string, string> processPreRequest(IInvocation invocation)
        {
            AesCryptoHelper aesCryptoHelper = new AesCryptoHelper
            {
                Key = GlobalConfigUtil.DbProxy_HashKey,
                Iv = GlobalConfigUtil.DbProxy_HashIv
            };

            Type type = invocation.TargetType;
            string dllName = type.Assembly.ManifestModule.Name;
            var request = new DbProxyRequest
            {
                AssemblyName = dllName.Substring(0, dllName.Length - 4),
                FullName = type.FullName,
                MethodName = invocation.Method.Name
            };

            if (invocation.Arguments != null)
            {
                request.Args = new Dictionary<int, object>();
                for (int i = 0; i < invocation.Arguments.Length; i++)
                {
                    request.Args.Add(i, invocation.Arguments[i]);
                }
            }

            string json = JsonConvert.SerializeObject(request);
            string encryptData = aesCryptoHelper.Encrypt(json);

            IDictionary<string, string> formBody = new Dictionary<string, string>();
            formBody.Add(nameof(DbProxyRequest), encryptData);

            return formBody;
        }

        private string processRequest(IDictionary<string, string> formBody, int timeoutSec)
        {
            string url = GlobalConfigUtil.DbProxy_Url;

            NetworkHelper networkHelper = new NetworkHelper
            {
                DefaultTimeout = timeoutSec
            };

            return networkHelper.DoRequestWithUrlEncode(url, formBody);
        }

        private object processResult(string response, Type returnType)
        {
            AesCryptoHelper aesCryptoHelper = new AesCryptoHelper
            {
                Key = GlobalConfigUtil.DbProxy_HashKey,
                Iv = GlobalConfigUtil.DbProxy_HashIv
            };

            string decryptData = aesCryptoHelper.Decrypt(response);
            var dataResult = JsonConvert.DeserializeObject<DataResult<string>>(decryptData);
            if (!dataResult.IsSuccess)
            {
                throw new Exception(dataResult.RtnData ?? dataResult.RtnMsg);
            }

            return JsonConvert.DeserializeObject(dataResult.RtnData, returnType);
        }
    }
}
