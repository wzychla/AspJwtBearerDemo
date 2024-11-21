using JwtDemo.Models.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtDemo.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Logon()
        {
            var model = new LogonModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logon(LogonModel model)
        {
            if ( this.ModelState.IsValid )
            {
                if ( model.Username == model.Password )
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username)
                    };

                    // create identity
                    ClaimsIdentity identity   = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await this.HttpContext.SignInAsync( CookieAuthenticationDefaults.AuthenticationScheme, principal );

                    return Redirect( "/" );

                }
            }

            return View( model );
        }
    }
}
