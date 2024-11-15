using Microsoft.AspNetCore.Mvc;

namespace RASP_Redis.Controllers
{
    public class RedisSessionController : Controller
    {
        public IActionResult Index()
        {
            // Set session value
            HttpContext.Session.SetString("TestKey", "TestValue");

            // Get session value
            var sessionValue = HttpContext.Session.GetString("TestKey");

            return Content("Index", sessionValue);
        }
    }
}
