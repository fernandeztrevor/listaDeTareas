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
    public class TareaController : Controller
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
            var emailLogged = HttpContext.User.Identity.Name;
            ViewBag.usrLogged = await ctx.Usuarios.Where(x => x.Email == emailLogged).FirstOrDefaultAsync();

            ViewBag.Tareas = await ctx.Tareas.OrderBy(x => x.Fecha).Include(x => x.IdAsignadoNavigation)
                .Include(x => x.IdCreadorNavigation).Where(x => x.IdAsignadoNavigation.Email == emailLogged && x.Finalizada == false).ToListAsync();

            ViewBag.TareasFinalizadas = await ctx.Tareas.OrderBy(x => x.Fecha).Include(x => x.IdAsignadoNavigation)
                .Include(x => x.IdCreadorNavigation).Where(x => x.IdAsignadoNavigation.Email == emailLogged && x.Finalizada == true).ToListAsync();

            ViewBag.Usuarios = await ctx.Usuarios.OrderBy(x => x.IdUsuario).ToListAsync();

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
        public async Task<IActionResult> SetTarea()
        {

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
                if (usrLogged.IdRol == 2)
                {
                    tarea.IdAsignado = usrLogged.IdUsuario;
                }
                tarea.Fecha = System.DateTime.Now;
                tarea.Finalizada = false;

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

        public async Task<IActionResult> Modificar(int id)
        {
            var emailLogged = HttpContext.User.Identity.Name;
            ViewBag.usrLogged = await ctx.Usuarios.Where(x => x.Email == emailLogged).FirstOrDefaultAsync();
            var tarea = ctx.Tareas.Find(id);
            // o se puede usar ctx.Cliente.Where(x=>x.IdCliente == id).Single() SingleOfDefault(), el segundo es a prueba de que no exista
            ViewBag.Usuarios = await ctx.Usuarios.OrderBy(x => x.IdUsuario).ToListAsync();
            
            if (tarea == null)
            {
                return RedirectToAction("Index");
            }
            return View(tarea);
        }

        public async Task<IActionResult> Finalizar(int id)
        {
            var Tarea = ctx.Tareas.Find(id);

            if (Tarea == null)
            {
                return StatusCode(404);
            }
            else
            {
                Tarea.Finalizada = true;
                ctx.Tareas.Attach(Tarea);
                ctx.Entry(Tarea).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}