using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtDemo.Controllers
{
    public class TokenController : Controller
    {
        private IConfiguration _cfg { get; set; }
        public TokenController( IConfiguration cfg )
        {
            this._cfg = cfg;
        }

        [Authorize( AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme )]
        [HttpGet]
        public IActionResult Acquire()
        {
            var plainTextSecurityKey = _cfg["JwtBearerKey"];

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

            var claimsIdentity =
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Name, this.User.Identity.Name)
                });

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials
            };

            var tokenHandler          = new JwtSecurityTokenHandler();
            var plainToken            = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            return Content( signedAndEncodedToken );
        }
    }
}
