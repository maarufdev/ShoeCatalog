using Microsoft.AspNetCore.Mvc;
using ShoeCatalog.Domain.ViewModels.UserVM;
using ShoeCatalog.Services.Interfaces;

namespace ShoeCatalog.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            var isLoggedIn = await _userService.Login(login);

            if (isLoggedIn) 
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            return View(login);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            var isRegistered = await _userService.Register(register);
            if(isRegistered)
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            return View(register);
        }
    }
}
