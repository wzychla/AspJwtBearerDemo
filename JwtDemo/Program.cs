using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtDemo
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            builder.Services.AddControllersWithViews();
            builder.Services
                .AddAuthentication( CookieAuthenticationDefaults.AuthenticationScheme )
                .AddCookie( CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/Account/Logon";
                    options.SlidingExpiration = true;
                } )                
                .AddJwtBearer( cfg =>
                {
                    var plainTextSecurityKey = config["JwtBearerKey"];

                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));
                    var signingCredentials = new SigningCredentials(signingKey,SecurityAlgorithms.HmacSha256Signature);

                    cfg.RequireHttpsMetadata = false;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = signingKey,
                        NameClaimType = "name"
                    };

                    cfg.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = async context =>
                        {
                            var ex = context.Exception;
                            Console.WriteLine( ex.Message );
                        }
                    };
                } );


            var app = builder.Build();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute( "default", "{controller=Home}/{action=Index}/{Id?}" );

            app.Run();
        }
    }
}
