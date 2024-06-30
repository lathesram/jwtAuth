using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RandomApp1.Controllers
{
    [ApiController]
    public class TestController : BaseController
    {
        [HttpGet]
        [Authorize]
        public ActionResult<string> Get()
        {
            return Ok("Authenticated");
        }
    }
}