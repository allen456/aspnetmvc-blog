using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnetmvc_blog.Data
{
    public class LoggingFilter : ActionFilterAttribute
    {
        public LoggingFilter(DataContext context)
        {
            _context = context;
        }

        private readonly DataContext _context;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string actionName = context.RouteData.Values["action"].ToString();
            string controllerName = context.RouteData.Values["controller"].ToString();
            string parameters = string.Empty;
            var userName = string.Empty;
            // -- Get User
            if (context.HttpContext != null && context.HttpContext.User != null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                userName = context.HttpContext.User.Identity.Name;
            }
            else
            {
                var threadPincipal = Thread.CurrentPrincipal;
                if (threadPincipal != null && threadPincipal.Identity.IsAuthenticated)
                {
                    userName = threadPincipal.Identity.Name;
                }
            }
            // -- Get Parameters
            foreach (var _paramerters in context.ActionArguments)
            {
                parameters += string.Format("{0}={1}&", _paramerters.Key, _paramerters.Value);
            }
            if (userName == "")
            {
                userName = "";
            }
            AppLog _log = new()
            {
                AppLogId = Guid.NewGuid(),
                AppLogAddress = context.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString(),
                AppLogAction = actionName,
                AppLogController = controllerName,
                AppLogDate = DateTime.Now,
                AppLogUser = userName,
                AppLogQuery = parameters,
            };
            _context.AppLogs.Add(_log);
            _context.SaveChanges();
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
    }
}
