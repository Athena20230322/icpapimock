using ICP.Infrastructure.Abstractions.FilterProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Infrastructure.Core.Web.Frameworks.FilterProxy
{
    public class FilterProxy : IFilterProxy
    {
        public virtual void OnActionExecuted(ActionExecutedContext filterContext, params object[] args)
        {
        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext, params object[] args)
        {
        }

        public virtual void OnException(ExceptionContext filterContext, params object[] args)
        {
        }

        public virtual void OnResultExecuted(ResultExecutedContext filterContext, params object[] args)
        {
        }

        public virtual void OnResultExecuting(ResultExecutingContext filterContext, params object[] args)
        {
        }
    }
}
