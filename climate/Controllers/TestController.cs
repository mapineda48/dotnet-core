using Microsoft.AspNetCore.Mvc;

namespace Climate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string SayHello() => "Hello";
    }
}