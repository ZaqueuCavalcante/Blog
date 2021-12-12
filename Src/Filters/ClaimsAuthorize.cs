using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Filters
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimType, string claimValue) : base(typeof(ClaimsAuthorizeFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimsAuthorizeFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public ClaimsAuthorizeFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            var userIsAuthorized = context.HttpContext.User.Claims.Any(
                c => c.Type == _claim.Type && c.Value == _claim.Value
            );

            if (userIsAuthorized) return;

            context.Result = new StatusCodeResult(403);
        }
    }
}
