using System.Diagnostics;
using System.Web.Mvc;
using NLog;

namespace ThirdPartyEventEditor.Filters
{
    public class ExecutionTimeAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private Stopwatch _stopwatch;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            _stopwatch.Stop();
            string controllerName = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.Controller.ControllerContext.RouteData.Values["action"].ToString();
            _logger.Info(controllerName + " " + actionName + " Elapsed Time:" + _stopwatch.Elapsed);
        }
    }
}