using JwtDemo.Models.Home;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemo.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme )]
        public IActionResult Index()
        {
            var model = new IndexModel();
            model.Username = this.User.Identity.Name;
            return View(model);
        }
    }
}
