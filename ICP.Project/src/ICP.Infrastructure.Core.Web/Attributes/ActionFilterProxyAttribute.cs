using ICP.Infrastructure.Abstractions.FilterProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Infrastructure.Core.Web.Attributes
{
    public class ActionFilterProxyAttribute : ActionFilterAttribute
    {
        #region 公開屬性

        public ProxyType ProxyType { get; set; }

        /// <summary>
        /// 相依注入
        /// </summary>
        public IFilterProxyFactory IFilterProxyFactory
        {
            set
            {
                _filterProxy = value.Create(ProxyType);
            }
        }

        #endregion

        private IFilterProxy _filterProxy = null;
        private object[] _args = null;

        public ActionFilterProxyAttribute(params object[] args)
        {
            _args = args;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            _filterProxy.OnActionExecuted(filterContext, _args);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            _filterProxy.OnActionExecuting(filterContext, _args);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            _filterProxy.OnResultExecuted(filterContext, _args);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            _filterProxy.OnResultExecuting(filterContext, _args);
        }
    }
}
