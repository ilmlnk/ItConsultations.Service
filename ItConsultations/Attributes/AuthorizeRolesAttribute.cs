using ItConsultations.Business.SharedTypes.Enums.System;
using ItConsultations.Utilities.Guards;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ItConsultations.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeRolesAttribute : Attribute, IAuthorizationFilter
{
    private readonly UserRole[] _roles;

    public AuthorizeRolesAttribute(params UserRole[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        //Guard.That(user.Identity?.IsAuthenticated == true, nameof(user.Identity?.IsAuthenticated));

        var userRoleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
        Guard.NotNullOrEmpty(userRoleClaim, nameof(userRoleClaim));

        Guard.That(Enum.TryParse<UserRole>(userRoleClaim, out var userRole), nameof(userRole));

        Guard.That(_roles.Contains(userRole), nameof(userRole));
    }
} 