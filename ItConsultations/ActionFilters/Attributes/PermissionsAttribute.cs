namespace ItConsultations.ActionFilters.Attributes;

public class PermissionsAttribute : TypeFilterAttribute
{
    public PermissionsAttribute(params string[] permissions) : base(typeof(PermissionsFilter))
    {
        Arguments = new object[] { permissions };
    }

    public class PermissionsFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissions = context.Filters.Get<PermissionsAttribute>().Permissions;
        }
    }
}