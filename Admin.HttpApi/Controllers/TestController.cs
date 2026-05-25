using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Admin.HttpApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : AbpControllerBase
    {
        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("Hello World!");
        }
    }
}
