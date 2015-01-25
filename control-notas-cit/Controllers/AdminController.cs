using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using control_notas_cit.Models.Repositorios;
using control_notas_cit.Models.Entidades;
using control_notas_cit.Models.ViewModels;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.Owin;
using control_notas_cit.Helpers;
using System.Data;

namespace control_notas_cit.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext AppContext;
        private IRepositorioGenerico<Proyecto> repoProyectos = null;
        private IRepositorioGenerico<ApplicationUser> repoUsers = null;
        private string profesor_rol_id;
        private string admin_rol_id;
        
        public AdminController()
        {
            // Obtengo el contexto que OWIN creó al iniciar la aplicación
            AppContext = ApplicationDbContext.GetDBContext();

            // Se lo paso a mi repositorio
            this.repoProyectos = new RepositorioGenerico<Proyecto>(AppContext);
            this.repoUsers = new RepositorioGenerico<ApplicationUser>(AppContext);

            // Necesito el id del rol del profesor para usarlo en este controlador
            this.profesor_rol_id = (AppContext.Roles.Where(x => x.Name == "Profesor")).Select(y => y.Id).Single();
            this.admin_rol_id = (AppContext.Roles.Where(x => x.Name == "Admin")).Select(y => y.Id).Single();
        }

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View(repoProyectos.SelectAll());
        }

        //
        // GET: /Admin/Crear/
        // Crear proyectos
        public ActionResult Crear()
        {
            return View(new ProjectViewModel()
            {
                Profesores = new SelectList(GetProfesoresList(), "Value", "Text")
            });
        }

        //
        // POST: /Admin/Crear/
        // Crear proyectos
        [HttpPost]
        public ActionResult Crear(ProjectViewModel model)
        {
            model.Profesores = new SelectList(GetProfesoresList(), "Value", "Text");

            if (ModelState.IsValid)
            {
                var profesores = new List<ApplicationUser>();
                profesores.Add(UserManager.FindByEmail(model.ProfesorEmail));

                Proyecto p = new Proyecto()
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Profesores = profesores
                };
                repoProyectos.Insert(p);
                repoProyectos.Save();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        //
        // GET: /Admin/EditarProyecto/
        public ActionResult EditarProyecto(int? id)
        {
            if(id == null)
            {
                TempData["message"] = "No se pudo encontrar el proyecto";
                return RedirectToAction("Index");
            }

            var proyecto = repoProyectos.SelectById(id);

            if( proyecto == null )
            {
                TempData["message"] = "No se pudo encontrar el proyecto";
                return RedirectToAction("Index");
            }

            var model = new ProjectEditViewModel()
            {
                Id = proyecto.ProyectoID,
                Nombre = proyecto.Nombre,
                Descripcion = proyecto.Descripcion
            };

            return View(model);
        }

        //
        // POST: /Admin/EditarProyecto/
        [HttpPost]
        public ActionResult EditarProyecto(ProjectEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                var proyecto = repoProyectos.SelectById(model.Id);

                if(proyecto == null)
                {
                    ModelState.AddModelError("", "No se pudo encontrar el proyecto");
                    return View(model);
                }

                proyecto.Nombre = model.Nombre;
                proyecto.Descripcion = model.Descripcion;

                repoProyectos.Update(proyecto);
                repoProyectos.Save();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Admin/ListaAdministradores/
        public ActionResult ListaAdministradores()
        {
            var model = GetUserList(admin_rol_id);
            return View(model);
        }

        //
        // POST: /Admin/BorrarAdministrador/5
        [HttpPost]
        public async Task<ActionResult> BorrarAdministrador(string id)
        {
            if (id == null)
            {
                TempData["message"] = "No se pudo encontrar el usuario";
                return RedirectToAction("ListaAdministradores");
            }

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                TempData["message"] = "No se pudo encontrar el usuario";
                return RedirectToAction("ListaAdministradores");
            }
            var result = await UserManager.DeleteAsync(user);

            return RedirectToAction("ListaAdministradores");
        }

        //
        // GET: /Admin/ListaProfesores/
        public ActionResult ListaProfesores()
        {
            var model = GetUserList(profesor_rol_id);
            return View(model);
        }

        //
        // GET: /Admin/AgregarProfesor/
        public ActionResult AgregarProfesor()
        {
            return View(new ProfesorViewModel()
            {
                Proyectos = new SelectList(GetProyectosList(), "Value", "Text")
            });
        }

        //
        // POST: /Admin/AgregarProfesor/
        [HttpPost]
        public async Task<ActionResult> AgregarProfesor(ProfesorViewModel model)
        {
            model.Proyectos = new SelectList(GetProyectosList(), "Value", "Text");

            if (ModelState.IsValid)
            {
                ApplicationUser profesor = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Cedula = model.Cedula,
                    PhoneNumber = model.Telefono
                };

                if (model.ProyectoID != null)
                {
                    Proyecto p = repoProyectos.SelectAll().Where(pr => pr.ProyectoID == model.ProyectoID).Single();
                    profesor.Proyecto = p;
                }

                var profesorResult = await UserManager.CreateAsync(profesor, model.PasswordHash);

                if (profesorResult.Succeeded)
                {
                    var roleResult = await UserManager.AddToRoleAsync(profesor.Id, RoleType.Profesor);

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", roleResult.Errors.First());
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", profesorResult.Errors.First());
                    return View(model);
                }

                return RedirectToAction("ListaProfesores");
            }

            return View(model);
        }

        //
        // GET: /Admin/EditarProfesor/1
        public async Task<ActionResult> EditarProfesor(string id)
        {
            if (id == null)
            {
                TempData["message"] = "No se pudo encontrar el profesor";
                return RedirectToAction("ListaProfesores");
            }
            var profesor = await UserManager.FindByIdAsync(id);
            if (profesor == null)
            {
                TempData["message"] = "No se pudo encontrar el profesor";
                return RedirectToAction("ListaProfesores");
            }

            var model = new ProfesorEditViewModel()
            {
                Id = profesor.Id,
                Nombre = profesor.Nombre,
                Apellido = profesor.Apellido,
                Email = profesor.Email,
                Telefono = profesor.PhoneNumber,
                Cedula = profesor.Cedula
            };

            if (profesor.Proyecto != null)
            {
                model.Proyectos = new SelectList(GetProyectosList(), "Value", "Text", profesor.Proyecto.ProyectoID);
            }
            else
            {
                model.Proyectos = new SelectList(GetProyectosList(), "Value", "Text");
            }

            return View(model);
        }

        //
        // POST: /Admin/EditarProfesor/1
        [HttpPost]
        public async Task<ActionResult> EditarProfesor(ProfesorEditViewModel model)
        {
            model.Proyectos = new SelectList(GetProyectosList(), "Value", "Text");

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    TempData["message"] = "No se pudo encontrar el profesor";
                    return RedirectToAction("ListaProfesores");
                }

                user.UserName = model.Email;
                user.Email = model.Email;
                user.Nombre = model.Nombre;
                user.Apellido = model.Apellido;
                user.Cedula = model.Cedula;
                user.PhoneNumber = model.Telefono;

                if (model.ProyectoID != null)
                {
                    user.Proyecto = repoProyectos.SelectAll().Where(p => p.ProyectoID == model.ProyectoID).Single();
                }
                else
                {
                    user.Proyecto = null;
                }

                var result = await UserManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(model);
                }
                return RedirectToAction("ListaProfesores");
            }
            ModelState.AddModelError("", "Algo falló.");
            return View(model);
        }

        //
        // POST: /Admin/BorrarProfesor/5
        [HttpPost]
        public async Task<ActionResult> BorrarProfesor(string id)
        {
            if (id == null)
            {
                TempData["message"] = "No se pudo encontrar el profesor";
                return RedirectToAction("ListaProfesores");
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["message"] = "No se pudo encontrar el profesor";
                return RedirectToAction("ListaProfesores");
            }
            var result = await UserManager.DeleteAsync(user);

            return RedirectToAction("ListaProfesores");
        }

        //
        // GET: /Admin/Celulas/2
        public ActionResult Celulas(int? id)
        {
            if(id == null)
            {
                TempData["message"] = "No se pudo encontrar el proyecto";
                return RedirectToAction("Index");
            }

            var proyecto = repoProyectos.SelectById(id);

            if(proyecto == null)
            {
                TempData["message"] = "No se pudo encontrar el proyecto";
                return RedirectToAction("Index");
            }

            return View(proyecto.Celulas.ToList());
        }

        //
        // GET: /Admin/AgregarAdministrador/
        public ActionResult AgregarAdministrador()
        {
            return View();
        }

        //
        // POST: /Admin/AgregarAdministrador/
        [HttpPost]
        public async Task<ActionResult> AgregarAdministrador(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(!model.PasswordHash.Equals(model.ConfirmarPassword))
                {
                    ModelState.AddModelError("", "Las contraseñas no coinciden.");
                    return View(model);
                }

                ApplicationUser admin = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Cedula = model.Cedula,
                    PhoneNumber = model.Telefono
                };
                
                var adminResult = await UserManager.CreateAsync(admin, model.PasswordHash);

                if (adminResult.Succeeded)
                {
                    var roleResult = await UserManager.AddToRoleAsync(admin.Id, "Admin");

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", roleResult.Errors.First());
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminResult.Errors.First());
                    return View(model);
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Admin/ExportarNotasCSV/
        public ActionResult ExportarNotasCSV(int? id_proyecto)
        {
            if(id_proyecto == null)
            {
                TempData["message"] = "No se pudo encontrar el proyecto";
                return RedirectToAction("Index");
            }

            var proyecto = repoProyectos.SelectById(id_proyecto);

            if (proyecto == null)
            {
                TempData["message"] = "No se pudo encontrar el proyecto";
                return RedirectToAction("Index");
            }

            var currCalendario = proyecto.Calendarios.Where(c => c.CalendarioID == proyecto.CalendarioActualID)
                .SingleOrDefault();
            bool puedeDescargar = currCalendario != null ? currCalendario.Finalizado == true : false;

            if(puedeDescargar)
            {
                var headers = new List<string> { "Nombre", "Apellido", "Cedula", "Telefono", "Email", "Proyecto", "Celula", "Nota Final" };
                var alumnosData = proyecto.Celulas.SelectMany(c => c.Alumnos)
                    .Where(a => a.Notas.Where(n => n.Calendario.CalendarioID == currCalendario.CalendarioID).SingleOrDefault() != null)
                                    .Select(alumno => new DataAlumnoExport
                                    {
                                        Nombre = alumno.Nombre,
                                        Apellido = alumno.Apellido,
                                        Cedula = alumno.Cedula,
                                        Email = alumno.Email,
                                        Telefono = alumno.Telefono,
                                        Proyecto = alumno.Celula.Proyecto.Nombre,
                                        Celula = alumno.Celula.Nombre,
                                        Nota_Final = (float)(Math.Round((float)(alumno.Notas.Where(n => n.Calendario.CalendarioID == proyecto.CalendarioActualID).SingleOrDefault().Nota_Final)))
                                    }).ToList();

                DataTable data = ExportHelper.CreateDataTable(headers, alumnosData);

                return new CsvActionResult(data) { FileDownloadName = string.Format("NotasProyectos-{0:dd-MM-yyyy-HH-mm-ss}.csv", DateTime.Now) };             
            }
            else
            {
                TempData["message"] = "No puedes descargar el archivo";
                return RedirectToAction("Index");
            }
        }

        public ActionResult ExportarAsistenciasCSV(int? id_proyecto)
        {
            if(id_proyecto == null)
            {
                TempData["message"] = "No se encontro el proyecto";
                return RedirectToAction("Index");
            }

            var proyecto = repoProyectos.SelectById(id_proyecto);

            if (proyecto == null)
            {
                TempData["message"] = "No se pudo encontrar el proyecto";
                return RedirectToAction("Index");
            }

            var currCalendario = proyecto.Calendarios.Where(c => c.CalendarioID == proyecto.CalendarioActualID)
                .SingleOrDefault();

            if(currCalendario == null)
            {
                TempData["message"] = "El proyecto no cuenta con ningun calendario asignado";
                return RedirectToAction("Index");
            }

            var semanaActual = currCalendario.Semanas.Where(s => s.SemanaID == currCalendario.SemanaActualID).SingleOrDefault();

            if (semanaActual == null)
            {
                TempData["message"] = "El calendario no tiene una semana asignada";
                return RedirectToAction("Index");
            }

            var nSemanas = semanaActual.NumeroSemana < 12 ? (semanaActual.NumeroSemana - 1) : semanaActual.NumeroSemana;

            if(nSemanas == 0)
            {
                TempData["message"] = "No se han enviado asistencias";
                return RedirectToAction("Index");
            }

            var headers = new List<string> { "Nombre", "Apellido", "Cedula", "Proyecto" };

            for(int index = 1; index <= nSemanas; index++)
            {
                headers.Add(String.Format("Semana {0}", index));
            }

            var alumnosData = proyecto.Celulas.SelectMany(c => c.Alumnos)
                                    .Select(a => new DataAsistencias
                                    {
                                        Nombre = a.Nombre,
                                        Apellido = a.Apellido,
                                        Cedula = a.Cedula,
                                        Proyecto = proyecto.Nombre,
                                        Asistencias = a.Asistencias.Where(p => p.Semana.Calendario.CalendarioID == currCalendario.CalendarioID)
                                                        .OrderBy(asistencia => asistencia.Semana.NumeroSemana).ToList()
                                    }).ToList();

            DataTable data = ExportHelper.CreateAsistenciasDataTable(headers, alumnosData, nSemanas);

            return new CsvActionResult(data) { FileDownloadName = string.Format("AsistenciaProyectos-{0:dd-MM-yyyy-HH-mm-ss}-Semana{1}.csv", DateTime.Now, nSemanas) };             
        }
        
        private List<SelectListItem> GetProyectosList()
        {
            List<SelectListItem> proyectos = repoProyectos.SelectAll()
                .Select(p => new SelectListItem()
                {
                    Value = p.ProyectoID.ToString(),
                    Text = p.Nombre
                })
                .ToList();

            return proyectos;
        }

        private List<SelectListItem> GetProfesoresList()
        {
            List<SelectListItem> profesores = repoUsers.SelectAll().Where(x => x.Roles.Select(y => y.RoleId).Contains(profesor_rol_id) && x.Proyecto == null)
                                                    .Select(p => new SelectListItem()
                                                    {
                                                        Value = p.Email,
                                                        Text = String.Format("{0} {1}", p.Nombre, p.Apellido)
                                                    }).ToList();
             
            return profesores;
        }

        private List<ApplicationUser> GetUserList(string role)
        {
            return repoUsers.SelectAll()
                .Where(u => u.Roles.Select(r => r.RoleId).Contains(role))
                .OrderBy(p => p.Nombre)
                .ThenBy(p => p.Apellido)
                .ToList();
        }

        // Estos métodos permiten acceder a la información de los usuarios, aunque también se pueden obtener a través de la tabla Users
        // Sin embargo, UserManager y RoleManager tienen métodos asincronicos mucho más optimizados
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
    }
}