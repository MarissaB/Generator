using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Generator.Authorization
{
    public class AdministratorAuthHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Admins can do anything
            if (context.User.IsInRole(OperationNames.AdministratorsRole))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
