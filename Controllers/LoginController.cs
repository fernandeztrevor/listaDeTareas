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
                return BadRequest(new JObject(){
                    // {"StatusCode", 400},
                    // {"Message", "Error"}
                    new JProperty("StatusCode", 400),
                    new JProperty("Message", "Error")
                });
            }
            else
            {
                var result = await ctx.Usuarios.Include("IdRolNavigation").Where(x => x.Email == Usuario.Username).SingleOrDefaultAsync();

                if (result == null)
                {
                    return NotFound(new JObject(){
                    // {"StatusCode", 404},
                    // {"Message", "Usuario no encontrado"}
                    new JProperty("StatusCode", 404),
                    new JProperty("Message", "Usuario no encontrado")
                });
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
                        return Ok();
                    }
                    else
                    {
                        var response = new JObject(){
                            new JProperty("StatusCode", 403),
                            new JProperty("Message", "Usuario o contraseña incorrecta")
                        };
                        return StatusCode(403, response);
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