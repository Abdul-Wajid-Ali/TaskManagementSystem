using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.API.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
