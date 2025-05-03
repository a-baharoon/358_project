// Controllers/TestController.cs
using Microsoft.AspNetCore.Mvc;

namespace TeamSync.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(string name)
        {
            return RedirectToAction("Result", new { message = $"Hello, {name}!" });
        }

        public IActionResult Result(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}