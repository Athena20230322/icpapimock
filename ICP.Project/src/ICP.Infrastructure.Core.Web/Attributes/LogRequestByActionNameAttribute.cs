using System.Web.Mvc;

namespace ICP.Infrastructure.Core.Web.Attributes
{
    /// <summary>
    /// 寫入log直接存到/ApiRequestLog/{ControllerName}/{ActionName}/底下
    /// </summary>
    public class LogRequestByActionNameAttribute : LogRequestAttribute
    {
        private object _controllerName;
        private object _actionName;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _controllerName = filterContext.Controller.ControllerContext.RouteData.Values["controller"];
            _actionName = filterContext.Controller.ControllerContext.RouteData.Values["action"];

            Logger = LoggerFactory.CreateLogger($"ApiRequestLog/{_controllerName}/{_actionName}");

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            _controllerName = filterContext.Controller.ControllerContext.RouteData.Values["controller"];
            _actionName = filterContext.Controller.ControllerContext.RouteData.Values["action"];

            Logger = LoggerFactory.CreateLogger($"ApiRequestLog/{_controllerName}/{_actionName}");

            base.OnResultExecuted(filterContext);
        }
    }
}