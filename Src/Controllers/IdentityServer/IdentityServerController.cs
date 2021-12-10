using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.IdentityServer
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class IdentityServerController : ControllerBase
    {
        public IdentityServerController() {}

        [HttpGet]
        public ActionResult Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return new JsonResult(claims);
        }
    }
}
