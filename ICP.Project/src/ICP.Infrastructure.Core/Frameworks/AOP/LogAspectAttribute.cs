using ICP.Infrastructure.Abstractions;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Frameworks.Logging;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace ICP.Infrastructure.Core.Frameworks.AOP
{
    public class LogAspectAttribute : Attribute
    {
        protected ILogger Logger = null;

        public LogAspectAttribute(Type type) : base()
        {
            this.Logger = new NLogLogger(type);
        }

        public LogAspectAttribute(string name) : base()
        {
            this.Logger = new NLogLogger(name);
        }

        public void OnMethodExecuting(IMethodCallMessage context)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new
                {
                    context.MethodName,
                    context.Uri,
                    context.Args
                }, CustomJsonSerializerSettings.IgnoreException);

                WriteLog("OnMethodExecuting", json);
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(new
                {
                    context.MethodName,
                    context.Uri,
                    ex.Message
                }, CustomJsonSerializerSettings.IgnoreException);

                WriteLog("OnMethodExecuting", json, ex);
            }
        }

        public void OnMethodExecuted(IMethodReturnMessage context)
        {
            try
            {
                if (context.Exception == null)
                {
                    string json = JsonConvert.SerializeObject(context.ReturnValue, CustomJsonSerializerSettings.IgnoreException);
                    WriteLog("OnMethodExecuted", json);
                }
                else
                {
                    WriteLog("OnMethodExecuted", string.Empty, context.Exception);
                }
            }
            catch (Exception ex)
            {
                WriteLog("OnMethodExecuted", string.Empty, ex);
            }
        }

        public virtual void WriteLog(string type, string msg, Exception ex = null)
        {
            string template = "[{0}] {1}";
            if (ex == null)
            {
                Logger.Info(template, type, msg);
            }
            else
            {
                Logger.Error(ex, template, type, msg);
            }
        }
    }
}
