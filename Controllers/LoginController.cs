using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ListaDeTareas.Helper;
using ListaDeTareas.Models;
using ListaDeTareas.Models.ViewModel;

namespace ListaDeTareas.Controllers
{
    public class LoginController : Controller
    {

        ListaDeTareasCTX ctx;

        public LoginController(ListaDeTareasCTX _ctx)
        {
            ctx = _ctx;
        }
        public IActionResult Index()
        {
            return View();
        }

        [BindProperty]
        public UsuarioVM Usuario { get; set; }
        public async Task<IActionResult> Login()
        {

            if (!ModelState.IsValid)
            {
                return Problem(statusCode: 400);
            }
            else
            {
                var result = await ctx.Usuarios.Include("IdRolNavigation").Where(x => x.Email == Usuario.Username).SingleOrDefaultAsync();

                if (result == null)
                {                    
                    return Problem(statusCode: 404, title: "Usuario no encontrado");
                }
                else
                {
                    if (HashHelper.CheckHash(Usuario.Clave, result.Clave, result.Llave))
                    {
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.IdUsuario.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Email, result.Email));
                        identity.AddClaim(new Claim(ClaimTypes.Name, result.Email));

                        identity.AddClaim(new Claim("Rol", result.IdRol.ToString()));


                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties
                            {
                                ExpiresUtc = DateTime.Now.AddDays(1),
                                IsPersistent = true
                            });
                        return StatusCode(200, Usuario.Username);
                    }
                    else
                    {
                        return Problem(statusCode: 403, title: "Usuario o contraseña incorrecta");
                    }
                }
            }
            return View();
        }
    
        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync();
            return Redirect("/Login");
        }
    }
}