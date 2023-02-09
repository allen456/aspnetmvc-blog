using Microsoft.AspNetCore.Authorization;

namespace aspnetmvc_blog.Data
{
    public class AuthenticationPolicy : IAuthorizationRequirement
    {
        public AuthenticationPolicy() { }
    }
    public class AuthenticationPolicyHandler : AuthorizationHandler<AuthenticationPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthenticationPolicy requirement)
        {
            var userName = context.User.Identity.Name;
            if (userName != null && userName != "")
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
