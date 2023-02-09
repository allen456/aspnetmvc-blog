using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace aspnetmvc_blog.Data
{
    public class AuthenticationFilter : ActionFilterAttribute
    {
        public AuthenticationFilter(DataContext context, SocketManager manager)
        {
            _context = context;
            _manager = manager;
        }

        private readonly DataContext _context;
        private SocketManager _manager;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string actionName = context.RouteData.Values["action"].ToString();
            string controllerName = context.RouteData.Values["controller"].ToString();
            int returnType = 403;
            string parameters = string.Empty;
            var userName = string.Empty;
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
            foreach (var _paramerters in context.ActionArguments)
            {
                parameters = parameters + string.Format("{0}={1}&", _paramerters.Key, _paramerters.Value);
            }
            LibraryAccount _account = new LibraryAccount(_context);
            Guid userId = Guid.NewGuid();
            if (_context.Users.Where(w => w.UserName == userName).ToList().Any())
            {
                userId = _context.Users.Where(w => w.UserName == userName).ToList().Last().UserId;
            }
            if (!_account.IsUserAuthorized(userId, actionName, controllerName))
            {
                context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Error" },
                    { "type", returnType },
                    { "url", "/" + controllerName +"/"+actionName+"?"+parameters }
                });
            }
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
