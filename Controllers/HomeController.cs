using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ListaDeTareas.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ListaDeTareas.Helper;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace ListaDeTareas.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        ListaDeTareasCTX ctx;

        public HomeController(ListaDeTareasCTX _ctx)
        {
            ctx = _ctx;
        }

        public IActionResult Index()
        {
            //return View();
            return Redirect("/Tarea");
        }


        //[AllowAnonymous]
        public async Task<IActionResult> Registro()
        {
            return View();
        }

        
        [BindProperty]
        public Usuario usuario { get; set; }
        [HttpPost]
        public async Task<IActionResult> Registrar()
        {
            var result = await GetUsuario(usuario);
         
            if (result != null)
            {
                return BadRequest(ModelState.SelectMany(x => 
                        x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
            }else{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState.SelectMany(x => 
                        x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
                }else{

                    var hash = HashHelper.Hash(usuario.Clave);
                    usuario.Clave = hash.Password;
                    usuario.Llave = hash.Salt;
                    usuario.IdRol = 2;

                    ctx.Usuarios.Add(usuario);
                    await ctx.SaveChangesAsync();

                    usuario.Clave = "";
                    usuario.Llave = "";

                    return StatusCode(statusCode: 200, "Registro exitoso");
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<Usuario> GetUsuario(Usuario usuario)
        {
            return await ctx.Usuarios.Where(x => x.Email == usuario.Email)
                .SingleOrDefaultAsync();
        }

        
    }
}
