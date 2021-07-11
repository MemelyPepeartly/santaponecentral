using Microsoft.AspNetCore.Authorization;
using Search.Logic.Models.Common_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Survey.Api.Authorization
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "permissions" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            IEnumerable<PermissionClaim> scopes = context.User.Claims.Where(c => c.Type == "permissions").Select(c => new PermissionClaim{ Type = c.Type, Value = c.Value});

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s.Value == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
