using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Controllers
{
    public class SubscriptionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
