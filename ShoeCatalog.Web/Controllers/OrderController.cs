using Microsoft.AspNetCore.Mvc;

namespace ShoeCatalog.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            return View();
        }
    }
}
