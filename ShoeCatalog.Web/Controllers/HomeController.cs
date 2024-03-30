using Microsoft.AspNetCore.Mvc;
using ShoeCatalog.DataModels.ViewModels.Home;
using ShoeCatalog.DataModels.ViewModels.ShoeVM;
using ShoeCatalog.Repositories.Interfaces;
using ShoeCatalog.Services.Interfaces;
using ShoeCatalog.Web.Models;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ShoeCatalog.DataModels.Models;
using Microsoft.Identity.Client;

namespace ShoeCatalog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShoeServices _shoeService;

        public HomeController(IShoeServices shoeService)
        {
            _shoeService = shoeService;
        }

        public async Task<IActionResult> Index()
        {
            var shoeCards = await _shoeService.GetShoeListCard();

            var homeVM = new HomeVM
            {
                Cards = shoeCards.OrderBy(x => Guid.NewGuid()).Take(4).ToList(),
                Carousels = shoeCards.OrderBy(x => Guid.NewGuid()).Take(4).ToList()
            };

            return View(homeVM ?? new HomeVM());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}