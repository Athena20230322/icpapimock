using Castle.DynamicProxy;
using ICP.Infrastructure.Abstractions;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Frameworks.Logging;
using ICP.Infrastructure.Core.Models.Enums;
using Newtonsoft.Json;
using System;

namespace ICP.Infrastructure.Core.Frameworks.AOP
{
    public class LogInterceptor : IInterceptor
    {
        private readonly ILoggerFactory _loggerFactory = null;

        public LogInterceptor(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public virtual void Intercept(IInvocation invocation)
        {
            var logger = _loggerFactory.CreateLogger(invocation.TargetType);

            try
            {
                writeLog(logger, "OnMethodExecuting", new
                {
                    invocation.Method.Name,
                    invocation.Arguments,
                });

                invocation.Proceed();

                writeLog(logger, "OnMethodExecuted", new
                {
                    invocation.ReturnValue
                });
            }
            catch (Exception ex)
            {
                writeLog(logger, "OnMethodException", ex.Message, ex);
                throw ex;
            }
        }

        private void writeLog(Abstractions.Logging.ILogger logger, string type, object obj, Exception ex = null)
        {
            string template = "[{0}] {1}";
            string json = JsonConvert.SerializeObject(obj, CustomJsonSerializerSettings.IgnoreException);

            if (ex == null)
            {
                logger.Info(template, type, json);
            }
            else
            {
                logger.Error(ex, template, type, json);
            }
        }
    }
}
