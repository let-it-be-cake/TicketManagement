using System;
using System.Web.Mvc;
using NLog;

namespace ThirdPartyEventEditor.Filters
{
    public class ExceptionHandler : ActionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            _logger.Error($"{ex.Message}\n{ex.Source}\n{ex.StackTrace}");

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<string>(ex.Message),
            };
        }
    }
}