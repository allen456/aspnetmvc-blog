using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace aspnetmvc_blog.Data
{
    public class AuthenticationFilterAPI : ActionFilterAttribute
    {
        public AuthenticationFilterAPI(DataContext context, SocketManager manager)
        {
            _context = context;
            _manager = manager;
        }

        private readonly DataContext _context;
        private readonly SocketManager _manager;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var errorPage = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Error" },
                    { "type", "401" },
                });
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader == null)
            {
                context.Result = errorPage;
            }
            else
            {
                if (!authHeader.StartsWith("Basic"))
                {
                    context.Result = errorPage;
                }
                else
                {
                    string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                    int seperatorIndex = usernamePassword.IndexOf(':');
                    var username = usernamePassword.Substring(0, seperatorIndex);
                    var password = usernamePassword.Substring(seperatorIndex + 1);
                    LibraryAccount _account = new LibraryAccount(_context);
                    string actionName = context.RouteData.Values["action"].ToString();
                    string controllerName = context.RouteData.Values["controller"].ToString();
                    bool loginTest = !_account.IsLoginCorrect(username, password);
                    bool authTest = !_account.IsUserAuthorized(_context.Users.Where(w => w.UserName == username).ToList().Last().UserId, actionName, controllerName);
                    if (loginTest && authTest)
                    {
                        context.Result = errorPage;
                    }
                }
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
