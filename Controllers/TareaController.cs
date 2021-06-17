using ListaDeTareas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaDeTareas.Controllers
{
    [Authorize]
    public class TareaController : Controller
    {
        ListaDeTareasCTX ctx;

        public TareaController(ListaDeTareasCTX _ctx)
        {
            ctx = _ctx;
        }

        public async Task<IActionResult> Index()
        {
            var emailLogged = HttpContext.User.Identity.Name;

            ViewBag.usrLogged = await GetUserLogged(emailLogged);

            ViewBag.Tareas = await GetTareas(emailLogged);

            ViewBag.PendientesAsignadas = await GetTareasPendientesAsignadas(emailLogged);

            ViewBag.TareasFinalizadas = await GetTareasFinalizadas(emailLogged);

            ViewBag.Usuarios = await GetUsuarios();

            Tarea Tarea = new Tarea();
            return View(Tarea);
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

            var _Tarea = await GetTarea(tarea);

            var emailLogged = HttpContext.User.Identity.Name;
            var usrLogged = await GetUserLogged(emailLogged);


            if (_Tarea == null)
            {
                if (usrLogged.IdRol == 2)
                {
                    tarea.IdAsignado = usrLogged.IdUsuario;
                    tarea.Bloqueada = true;
                }

                tarea.IdCreador = usrLogged.IdUsuario;

                tarea.Fecha = System.DateTime.Now;
                tarea.Finalizada = false;

                ctx.Tareas.Add(tarea);
            }
            else
            {

                if (!verificarPermisos(_Tarea, usrLogged))
                {
                    return Problem(statusCode: 400, title: "Modificacion Denegada. Sin permisos!!!");
                }
                if (usrLogged.IdRol == 1)
                {
                    _Tarea.IdAsignado = tarea.IdAsignado;
                    _Tarea.Bloqueada = tarea.Bloqueada;
                }

                _Tarea.Titulo = tarea.Titulo;
                _Tarea.Descripcion = tarea.Descripcion;


                ctx.Tareas.Attach(_Tarea);
                ctx.Entry(_Tarea).State = EntityState.Modified;
            }

            await ctx.SaveChangesAsync();

            return StatusCode(statusCode: 200, "Operacion exitosa");
        }

        public async Task<IActionResult> Modificar(int id)
        {
            var emailLogged = HttpContext.User.Identity.Name;
            ViewBag.usrLogged = await GetUserLogged(emailLogged);

            var tarea = ctx.Tareas.Find(id);

            ViewBag.Usuarios = await GetUsuarios();

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

        public static bool verificarPermisos(Tarea tarea, Usuario usuario)
        {
            if (tarea.Bloqueada == true && usuario.IdUsuario != tarea.IdCreador)
            {
                return false;
            }

            return true;
        }


        public async Task<Usuario> GetUserLogged(string emailLogged)
        {
            return await ctx.Usuarios.Where(x => x.Email == emailLogged).FirstOrDefaultAsync();
        }

        public async Task<List<Tarea>> GetTareas(string emailLogged)
        {
            return await ctx.Tareas.OrderBy(x => x.Fecha).Include(x => x.IdAsignadoNavigation)
                .Include(x => x.IdCreadorNavigation).Where(x => x.IdAsignadoNavigation.Email == emailLogged && x.Finalizada == false).ToListAsync();
        }

        public async Task<List<Tarea>> GetTareasPendientesAsignadas(string emailLogged)
        {
            return await ctx.Tareas.OrderBy(x => x.Fecha).Include(x => x.IdAsignadoNavigation)
            .Include(x => x.IdCreadorNavigation).Where(x => x.IdCreadorNavigation.Email == emailLogged && x.Finalizada == false).ToListAsync();
        }

        public async Task<List<Tarea>> GetTareasFinalizadas(string emailLogged)
        {
            return await ctx.Tareas.OrderBy(x => x.Fecha).Include(x => x.IdAsignadoNavigation)
                .Include(x => x.IdCreadorNavigation).Where(x => x.IdAsignadoNavigation.Email == emailLogged && x.Finalizada == true).ToListAsync();
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            return await ctx.Usuarios.OrderBy(x => x.IdUsuario).ToListAsync();
        }

        public async Task<Tarea> GetTarea(Tarea tarea)
        {
            return await ctx.Tareas.Where(x => x.IdTarea == tarea.IdTarea).SingleOrDefaultAsync();
        }

    }
}