using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Infrastructure.Abstractions.FilterProxy
{
    public interface IFilterProxy
    {
        void OnActionExecuted(ActionExecutedContext filterContext, params object[] args);

        void OnActionExecuting(ActionExecutingContext filterContext, params object[] args);

        void OnResultExecuted(ResultExecutedContext filterContext, params object[] args);

        void OnResultExecuting(ResultExecutingContext filterContext, params object[] args);

        void OnException(ExceptionContext filterContext, params object[] args);
    }
}
