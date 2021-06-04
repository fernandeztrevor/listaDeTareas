using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ListaDeTareas.Helper;
using ListaDeTareas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace listaDeTareas.Controllers
{
    [Authorize]
    public class TareaController: Controller
    {
        ListaDeTareasCTX ctx;

        public TareaController(ListaDeTareasCTX _ctx)
        {
            ctx = _ctx;
        }

        public IActionResult Index()
        {
            return Redirect("Tarea/Propias");
        }

        public async Task<IActionResult> Propias()
        {           
            ViewBag.Tareas = await ctx.Tareas.Include(x => x.IdAsignadoNavigation)
                .Include(x => x.IdCreadorNavigation).ToListAsync();
            
            ViewBag.Usuarios = await ctx.Usuarios.OrderBy(x => x.IdUsuario).ToListAsync();

            
            var emailLogged = HttpContext.User.Identity.Name;
            ViewBag.usrLogged = await ctx.Usuarios.Where(x => x.Email == emailLogged).FirstOrDefaultAsync();
            
            Tarea Tarea = new Tarea();
            return View(Tarea);
        }

        public IActionResult Todas()
        {
            return View();
        }


        [BindProperty]
        public Tarea tarea { get; set; }
        [HttpPost]
        public async Task<IActionResult> SetTarea(){

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var _Tarea = await ctx.Tareas.Where(x => x.IdTarea == tarea.IdTarea).AnyAsync();
            var emailLogged = HttpContext.User.Identity.Name;
            var usrLogged = await ctx.Usuarios.Where(x => x.Email == emailLogged).FirstOrDefaultAsync();

            if (!_Tarea)
            {
                tarea.IdCreador = usrLogged.IdUsuario;
                if(usrLogged.IdRol == 2){
                    tarea.IdAsignado = usrLogged.IdUsuario;
                }
                tarea.Fecha = System.DateTime.Now;


                ctx.Tareas.Add(tarea);
            }
            else
            {
                ctx.Tareas.Attach(tarea);
                ctx.Entry(tarea).State = EntityState.Modified;
            }

            await ctx.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}