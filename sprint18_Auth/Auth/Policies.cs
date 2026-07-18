using Microsoft.AspNetCore.Authorization;

namespace sprint18_Auth.Auth;

public static class Policies
{
    public const string RequireAdminRole = "RequireAdminRole";
    public const string RequireUserRole = "RequireUserRole";

    public static void AddPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(RequireAdminRole, policy =>
            policy.RequireRole(RoleNames.Admin));

        options.AddPolicy(RequireUserRole, policy =>
            policy.RequireRole(RoleNames.User));
    }
}