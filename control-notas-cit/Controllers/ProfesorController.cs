using control_notas_cit.Models.Entidades;
using control_notas_cit.Models.Repositorios;
using control_notas_cit.Models.ViewModels;
using Microsoft.AspNet.Identity;
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.Owin;
using control_notas_cit.Helpers;
using System.IO;
using LinqToExcel;
using control_notas_cit.Models.Servicios;

namespace control_notas_cit.Controllers
{
    [Authorize(Roles = "Profesor")]
    public class ProfesorController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public ProfesorController()
        {
            var AppContext = ApplicationDbContext.GetDBContext();
            _uow = new UnitOfWork(AppContext);
        }

        //
        // GET: /Profesor/Index/
        public ActionResult Index()
        {
            ProfesorIndexViewModel model = new ProfesorIndexViewModel();

            // Obtengo el usuario
            var currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return RedirectToAction("Logoff", "Account");
            }

            if (currentUser.Proyecto == null)
            {
                return RedirectToAction("Logoff", "Account");
            }

            // Utilizo el usuario para obtener los demas datos del modelo de la vista
            model.NombreProfesor = currentUser.Nombre + " " + currentUser.Apellido;
            model.Proyecto = GetCurrentProyecto();
            model.Profesores = GetCurrentProyecto().Profesores.Where(p => UserManager.IsInRole(p.Id, "Profesor")).Select(z => z.Nombre + " " + z.Apellido).ToList();

            // Calendario será null si la query no devuelve un valor, en el caso de que sea null, la vista mostrara un mensaje
            model.Calendario = _uow.RepositorioCalendarios.SelectAll()
                                .Where(c => c.CalendarioID == model.Proyecto.CalendarioActualID)
                                .SingleOrDefault();

            return View(model);
        }

        //
        // GET: /Profesor/EditarProyecto/
        //
        public ActionResult EditarProyecto()
        {
            var proyecto = GetCurrentProyecto();

            if (proyecto == null)
            {
                TempData["message"] = "Proyecto no encontrado";
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
        // POST: /Profesor/EditarProyecto/
        [HttpPost]
        public ActionResult EditarProyecto(ProjectEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var proyecto = _uow.RepositorioProyectos.SelectById(model.Id);

                if (proyecto == null)
                {
                    ModelState.AddModelError("", "No se pudo encontrar el proyecto");
                    return View(model);
                }

                proyecto.Nombre = model.Nombre;
                proyecto.Descripcion = model.Descripcion;

                _uow.RepositorioProyectos.Update(proyecto);
                _uow.Save();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Profesor/AgregarCalendario/
        public ActionResult AgregarCalendario()
        {            
            CalendarioViewModel model = new CalendarioViewModel()
            {
                FechaInicio = DateTime.Now
            };

            return View(model);
        }

        //
        // POST: /Profesor/AgregarCalendario/
        [HttpPost]
        public ActionResult AgregarCalendario(CalendarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                Calendario calendario = new Calendario();

                // Cada calendario cuenta con 12 semanas
                int numeroSemanas = 12;

                calendario.FechaInicio = model.FechaInicio;
                calendario.FechaFinal = model.FechaInicio.AddDays(numeroSemanas * 7);

                calendario.Semanas = new List<Semana>();

                for(int index = 1; index <= numeroSemanas; index++) 
                {
                    Semana semana = new Semana()
                    {
                        Fecha = model.FechaInicio.AddDays(index * 7),
                        NumeroSemana = index
                    };
                    calendario.Semanas.Add(semana);
                }

                var user = GetCurrentUser();

                if (user == null)
                {
                    TempData["message"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                var proyecto = user.Proyecto;

                if (proyecto == null)
                {
                    TempData["message"] = "Proyecto no encontrado";
                    return RedirectToAction("Index");
                }

                calendario.Proyecto = proyecto;

                // Notas
                float suma = (float)model.Notas_Asistencias_Valor + (float)model.Notas_Minutas_Valor + (float)model.Notas_Presentacion_Valor;

                if (suma != 10.0f)
                {
                    ModelState.AddModelError("", "La sumatoria de las notas debe dar 10");
                    return View(model);
                }

                calendario.Notas_Minutas_Valor = model.Notas_Minutas_Valor;
                calendario.Notas_Asistencias_Valor = model.Notas_Asistencias_Valor;
                calendario.Notas_Evaluacion_Final_Valor = model.Notas_Presentacion_Valor;

                _uow.RepositorioCalendarios.Insert(calendario);
                _uow.Save();

                Semana sem = calendario.Semanas.Where(s => s.NumeroSemana == 1).Single();
                sem.Iniciada = true;
                calendario.SemanaActualID = sem.SemanaID;
                proyecto.CalendarioActualID = calendario.CalendarioID;

                _uow.RepositorioProyectos.Update(proyecto);
                _uow.Save();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Profesor/EditarSemana/
        public ActionResult EditarSemana(int id)
        {
            Semana s = _uow.RepositorioSemanas.SelectById(id);
            var model = new SemanaViewModel()
            {
                SemanaID = s.SemanaID,
                Fecha = s.Fecha,
                Actividad = s.Actividad,
                Descripcion = s.Descripcion,
                NumeroSemana = s.NumeroSemana
            };
            return View(model);
        }

        //
        // POST: /Profesor/EditarSemana/
        [HttpPost]
        public ActionResult EditarSemana(SemanaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Semana semana = _uow.RepositorioSemanas.SelectById(model.SemanaID);

                if (semana == null)
                {
                    ModelState.AddModelError("", "No se pudo encontrar esta semana");
                    return View(model);
                }

                semana.Actividad = model.Actividad;
                semana.Descripcion = model.Descripcion;

                _uow.RepositorioSemanas.Update(semana);
                _uow.Save();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Profesor/FinalizarSemana/2
        public ActionResult FinalizarSemana(int id)
        {
            Semana semana = _uow.RepositorioSemanas.SelectById(id);
            var alumnosSinAsistencias = GetCurrentProyecto()
                .Celulas
                .SelectMany(x => x.Alumnos)
                .Where(a => a.Asistencias.Where(c => c.Semana.SemanaID == semana.SemanaID).SingleOrDefault() == null)
                .ToList();

            if (semana.SemanaID != GetCurrentSemana().SemanaID)
            {
                return RedirectToAction("Index");
            }

            var model = new FinalizarSemanaViewModel()
            {
                Semana = semana,
                MinutasPorAprobar = semana.Minutas.Where(m => m.Aprobada == false).ToList(),
                AlumnosAsistencias = alumnosSinAsistencias
            };

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // POST: /Profesor/FinalizarSemana/
        [HttpPost]
        public ActionResult FinalizarSemana(int? id)
        {
            if (GetCurrentCalendario().IsLastWeek == true || GetCurrentCalendario().Finalizado == true)
            {
                return RedirectToAction("Index");
            }

            if (id != null)
            {
                Semana semana = _uow.RepositorioSemanas.SelectById(id);
                List<Celula> celulas = _uow.RepositorioCelulas.SelectAll().Where(c => c.Proyecto.ProyectoID == GetCurrentProyecto().ProyectoID).ToList();

                if (semana.Iniciada == true && semana.Finalizada == false)
                {
                    semana.Finalizada = true;
                    _uow.RepositorioSemanas.Update(semana);

                    foreach (Celula celula in celulas)
                    {
                        var asistenciasSemanaActual = celula.Asistencias.Where(a => a.Semana.SemanaID == semana.SemanaID).ToList();

                        if (asistenciasSemanaActual.Count != celula.Alumnos.Count)
                        {
                            foreach (Alumno alumno in celula.Alumnos)
                            {
                                if (alumno.Asistencias.Where(a => a.Semana.SemanaID == semana.SemanaID).Count() == 0)
                                {
                                    Asistencia asistencia = new Asistencia()
                                    {
                                        Alumno = alumno,
                                        Semana = semana,
                                        Celula = celula
                                    };

                                    _uow.RepositorioAsistencias.Insert(asistencia);
                                }
                            }

                            _uow.Save();
                        }
                    }

                    // Numero de semanas --- CAMBIAR
                    if (semana.NumeroSemana < 12)
                    {
                        Semana proximaSemana = GetCurrentCalendario().Semanas.Where(s => s.NumeroSemana == (semana.NumeroSemana + 1)).Single();
                        proximaSemana.Iniciada = true;
                        proximaSemana.Calendario.SemanaActualID = proximaSemana.SemanaID;

                        _uow.RepositorioSemanas.Update(proximaSemana);
                    }
                    else
                    {
                        Calendario calendario = GetCurrentCalendario();
                        calendario.IsLastWeek = true;
                        _uow.RepositorioCalendarios.Update(calendario);
                        _uow.RepositorioCalendarios.Save();

                        // Sacar esto de la configuracion al crear el calendario        
                        float minutasPorcalendario = 12.0f;
                        float notaMinutas = (float)calendario.Notas_Minutas_Valor;
                        float asistenciasPorCalendario = 12.0f;
                        float notaAsistencias = (float)calendario.Notas_Asistencias_Valor;
                        // Creo las notas
                        foreach (Celula celula in celulas)
                        {
                            int minutasCelula = celula.Minutas.Where(m => m.Semana.Calendario.CalendarioID == calendario.CalendarioID && m.Aprobada == true).ToList().Count;

                            foreach (Alumno alumno in celula.Alumnos)
                            {
                                int asistenciasAlumno = alumno.Asistencias.Where(a => a.Semana.Calendario.CalendarioID == calendario.CalendarioID && a.Asistio == true).ToList().Count;

                                Nota nota = new Nota()
                                {
                                    Nota_Minutas = (notaMinutas / minutasPorcalendario) * minutasCelula,
                                    Nota_Asistencia = (notaAsistencias / asistenciasPorCalendario) * asistenciasAlumno,
                                    Alumno = alumno,
                                    Calendario = calendario
                                };

                                _uow.RepositorioNotas.Insert(nota);
                            }
                        }
                    }

                    _uow.Save();
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult AsistenciaSemana(AsistenciaSemanaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var semana = GetCurrentSemana();

                foreach(int id_alumno in model.ID_Alumnos)
                {
                    var alumno = _uow.RepositorioAlumnos.SelectById(id_alumno);

                    Asistencia asistencia = new Asistencia()
                    {
                        Alumno = alumno,
                        Semana = semana,
                        Celula = alumno.Celula,
                        Asistio = true
                    };

                    _uow.RepositorioAsistencias.Insert(asistencia);
                }

                _uow.Save();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        //
        // GET: /Profesor/CargarNotasFinales/
        public ActionResult CargarNotasFinales()
        {
            if (GetCurrentCalendario().IsLastWeek != true || GetCurrentCalendario().Finalizado == true)
            {
                TempData["message"] = "No puedes cargar las notas finales hasta el final del calendario";
                return RedirectToAction("Index");
            }
          
            return View();
        }

        [HttpPost]
        public ActionResult CargarNotasFinales(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.Equals(".xls") || extension.Equals(".xlsx") || extension.Equals(".csv"))
                {
                    try
                    {
                        // upload the file
                        string fileName = string.Format("Excel-{0:dd-MM-yyyy-HH-mm-ss}{1}", DateTime.Now, extension);
                        string path = Path.Combine(Server.MapPath("~/ExcelTemp"), fileName);
                        file.SaveAs(path);

                        // get the file
                        var excel = new ExcelQueryFactory(path);
                        var notas = excel.Worksheet<ImportNotasFinales>().ToList();
                        var notasDistinct = notas.GroupBy(n => n.Cedula).Select(x => x.First()).ToList();

                        // delete the file
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        var proyecto = GetCurrentProyecto();
                        var calendario = GetCurrentCalendario();

                        var alumnosExistentes = proyecto.Celulas.SelectMany(c => c.Alumnos).Count();
                        if (notasDistinct.Count == alumnosExistentes)
                        {
                            // send or do something with data
                            foreach (ImportNotasFinales row in notas)
                            {
                                var alumno = proyecto.Celulas.SelectMany(a => a.Alumnos)
                                    .Where(a => a.Cedula == ModelHelpers.LimpiarPuntos(row.Cedula))
                                    .SingleOrDefault();

                                if (alumno == null)
                                {
                                    TempData["message"] = "Debes incluir las notas de todos los alumnos del proyecto";
                                    return View();
                                }

                                var nota = alumno.Notas.Where(n => n.Calendario.CalendarioID == calendario.CalendarioID)
                                    .SingleOrDefault();
                                
                                if (nota != null)
                                {
                                    if (row.Nota <= calendario.Notas_Evaluacion_Final_Valor)
                                    {
                                        nota.Nota_EvaluacionFinal = row.Nota;
                                        nota.Nota_Final = nota.Nota_Minutas + nota.Nota_Asistencia + nota.Nota_EvaluacionFinal;

                                        _uow.RepositorioNotas.Update(nota);
                                    }
                                    else
                                    {
                                        TempData["message"] = string.Format("Todas las notas deben estar comprendidas entre 0 y {0}", calendario.Notas_Evaluacion_Final_Valor);
                                        return View();
                                    }
                                }
                            }

                            calendario.Finalizado = true;

                            _uow.RepositorioCalendarios.Update(calendario);
                            _uow.Save();
                        }
                        else
                        {
                            TempData["message"] = "Debes incluir las notas de todos los alumnos del proyecto";
                        }

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                ViewBag.Message = "Debes especificar algún archivo.";
            }

            return View();
        }
        
        //
        // GET: /Profesor/Celulas/
        public ActionResult Celulas()
        {
            return View(_uow.RepositorioCelulas.SelectAll().Where(c => c.Proyecto.ProyectoID == GetCurrentProyecto().ProyectoID).ToList());
        }

        //
        // GET: /Profesor/AgregarCelula/
        public ActionResult AgregarCelula()
        {
            return View(new CelulaViewModel()
            {
                Coordinadores = new MultiSelectList(GetCoordinadoresLibresList(), "Value", "Text")
            });
        }

        //
        // POST: /Profesor/AgregarCelula/
        [HttpPost]
        public ActionResult AgregarCelula(CelulaViewModel model)
        {
            model.Coordinadores = new MultiSelectList(GetCoordinadoresLibresList(), "Value", "Text");

            if (ModelState.IsValid)
            {
                List<ApplicationUser> coordinadores = _uow.RepositorioUsuarios.SelectAll().Where(u => model.CoordinadoresID.Contains(u.Id)).ToList();

                Celula celula = new Celula()
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Coordinadores = coordinadores,
                    Proyecto = GetCurrentProyecto()
                };

                _uow.RepositorioCelulas.Insert(celula);
                _uow.Save();

                return RedirectToAction("Celulas");
            }
            return View(model);
        }

        //
        // GET: /Profesor/EditarCelula/
        public ActionResult EditarCelula(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Celulas");
            }

            var celula = _uow.RepositorioCelulas.SelectById(id);

            if (celula == null)
            {
                return RedirectToAction("Celulas");
            }

            var model = new CelulaEditViewModel()
            {
                Id = celula.CelulaID,
                Nombre = celula.Nombre,
                Descripcion = celula.Descripcion
            };

            return View(model);
        }

        //
        // POST: /Profesor/EditarCelula/
        [HttpPost]
        public ActionResult EditarCelula(CelulaEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var celula = _uow.RepositorioCelulas.SelectById(model.Id);

                if (celula == null)
                {
                    ModelState.AddModelError("", "No se pudo encontrar la celula");
                    return View(model);
                }

                celula.Nombre = model.Nombre;
                celula.Descripcion = model.Descripcion;

                _uow.RepositorioCelulas.Update(celula);
                _uow.Save();

                return RedirectToAction("Celulas");
            }

            return View(model);
        }

        //
        // GET: /Profesor/ListaCoordinadores/
        public ActionResult ListaCoordinadores()
        {
            var model = GetCoordinadores().OrderBy(c => c.Nombre).ThenBy(c => c.Apellido).ToList();
            return View(model);
        }

        //
        // GET: /Profesor/AgregarCoordinador/
        public ActionResult AgregarCoordinador()
        {
            return View(new CoordinadorViewModel()
            {
                Celulas = new SelectList(GetCelulasList(), "Value", "Text")
            });
        }

        //
        // POST: /Profesor/AgregarCoordinador/
        [HttpPost]
        public async Task<ActionResult> AgregarCoordinador(CoordinadorViewModel model)
        {
            model.Celulas = new SelectList(_uow.RepositorioCelulas.SelectAll().Select(c => c.Nombre).ToList());

            if (ModelState.IsValid)
            {
                if (!model.ConfirmarPassword.Equals(model.PasswordHash))
                {
                    ModelState.AddModelError("", "Las contraseñas no coinciden");
                    return View(model);
                }
                ApplicationUser coordinador;

                coordinador = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Cedula = model.Cedula,
                    Proyecto = GetCurrentProyecto(),
                    PhoneNumber = model.Telefono
                };

                if (model.Celula != null)
                {
                    Celula celula = _uow.RepositorioCelulas.SelectAll().Where(c => c.CelulaID == Int32.Parse(model.Celula)).Single();
                    coordinador.Celula = celula;
                }

                var coordinadorResult = await UserManager.CreateAsync(coordinador, model.PasswordHash);

                if (coordinadorResult.Succeeded)
                {
                    var roleResult = await UserManager.AddToRoleAsync(coordinador.Id, "Coordinador");

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", roleResult.Errors.First());
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", coordinadorResult.Errors.First());
                    return View(model);
                }

                return RedirectToAction("ListaCoordinadores");
            }

            return View(model);
        }

        //
        // GET: /Profesor/EditarCoordinador/42
        public async Task<ActionResult> EditarCoordinador(string id)
        {
            if (id == null)
            {
                return RedirectToAction("ListaCoordinadores");
            }
            var coordinador = await UserManager.FindByIdAsync(id);
            if (coordinador == null)
            {
                return RedirectToAction("ListaCoordinadores");
            }

            var model = new CoordinadorEditViewModel()
            {
                Id = coordinador.Id,
                Nombre = coordinador.Nombre,
                Apellido = coordinador.Apellido,
                Email = coordinador.Email,
                Telefono = coordinador.PhoneNumber,
                Cedula = coordinador.Cedula
            };

            if (coordinador.Celula != null)
            {
                // bug
                model.Celulas = new SelectList(GetCelulasList(), "Value", "Text", coordinador.Celula.CelulaID.ToString());
            }
            else
            {
                model.Celulas = new SelectList(GetCelulasList(), "Value", "Text");
            }

            return View(model);
        }

        //
        // POST: /Profesor/EditarCoordinador/
        [HttpPost]
        public async Task<ActionResult> EditarCoordinador(CoordinadorEditViewModel model)
        {
            model.Celulas = new SelectList(GetCelulasList(), "Value", "Text");

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = model.Email;
                user.Email = model.Email;
                user.Nombre = model.Nombre;
                user.Apellido = model.Apellido;
                user.Cedula = model.Cedula;
                user.PhoneNumber = model.Telefono;

                if (model.CelulaID != null)
                {
                    user.Celula = _uow.RepositorioCelulas.SelectById(Int32.Parse(model.CelulaID));
                }
                else
                {
                    ModelState.AddModelError("", "Debes elegir una celula");
                    return View(model);
                }

                var result = await UserManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(model);
                }
                return RedirectToAction("ListaCoordinadores");
            }
            ModelState.AddModelError("", "Algo falló.");
            return View(model);
        }

        //
        // POST: /Profesor/BorrarCoordinador/42
        [HttpPost]
        public async Task<ActionResult> BorrarCoordinador(string id)
        {
            if (id == null)
            {
                return RedirectToAction("ListaCoordinadores");
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("ListaCoordinadores");
            }
            var result = await UserManager.DeleteAsync(user);

            return RedirectToAction("ListaCoordinadores");
        }

        //
        // GET: /Profesor/MinutasSemana/1
        public ActionResult MinutasSemana(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var semana = _uow.RepositorioSemanas.SelectById(id);

            if (semana == null)
            {
                return RedirectToAction("Index");
            }

            Proyecto proyecto = GetCurrentProyecto();
            Calendario calendario = GetCurrentCalendario();

            if (calendario == null)
            {
                return RedirectToAction("Index");
            }

            var model = new MinutaPartialViewModel()
            {
                Minutas = proyecto.Celulas.SelectMany(c => c.Minutas.Where(m => m.Semana.SemanaID == semana.SemanaID)).ToList(),
                CurrentSemana = calendario.Semanas.Where(s => s.SemanaID == calendario.SemanaActualID).Single()
            };
            return View("Minutas", model);
        }

        //
        // GET: /Profesor/MinutasCelula/2
        public ActionResult MinutasCelula(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var celula = _uow.RepositorioCelulas.SelectById(id);

            if (celula == null)
            {
                return RedirectToAction("Index");
            }

            Calendario calendario = GetCurrentCalendario();

            if (calendario == null)
            {
                TempData["message"] = "No hay abierto ningún calendario";
                return RedirectToAction("Index");
            }

            var model = new MinutaPartialViewModel()
            {
                Minutas = celula.Minutas.Where(m => m.Semana.Calendario.CalendarioID == calendario.CalendarioID).ToList(),
                CurrentSemana = calendario.Semanas.Where(s => s.SemanaID == calendario.SemanaActualID).Single()
            };

            return View("Minutas", model);
        }

        //
        // GET: /Profesor/AgregarProfesor/
        public ActionResult AgregarProfesor()
        {
            return View();
        }

        //
        // POST: /Profesor/AgregarProfesor/
        [HttpPost]
        public async Task<ActionResult> AgregarProfesor(ProfesorViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.ConfirmarPassword.Equals(model.PasswordHash))
                {
                    ModelState.AddModelError("", "Las contraseñas no coinciden.");
                    return View(model);
                }

                ApplicationUser profesor = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Cedula = model.Cedula,
                    PhoneNumber = model.Telefono,
                    Proyecto = GetCurrentProyecto()
                };

                var profesorResult = await UserManager.CreateAsync(profesor, model.PasswordHash);

                if (profesorResult.Succeeded)
                {
                    var roleResult = await UserManager.AddToRoleAsync(profesor.Id, "Profesor");

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

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Profesor/AprobarMinuta/4
        [HttpPost]
        public ActionResult AprobarMinuta(int? id_minuta)
        {
            if (id_minuta == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            Minuta minuta = GetCurrentProyecto().Celulas.SelectMany(c => c.Minutas.Where(m => m.MinutaID == id_minuta)).Single();

            if (minuta == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            if (GetCurrentSemana().SemanaID == minuta.Semana.SemanaID)
            {
                minuta.Aprobada = true;
            }

            _uow.RepositorioMinutas.Update(minuta);
            _uow.Save();

            return Redirect(Request.UrlReferrer.ToString());
        }

        //
        // GET: /Profesor/Asistencias/
        public ActionResult Asistencias()
        {
            List<Alumno> alumnos = GetAlumnosProyecto();

            List<Alumno> alumnosFiltrados = new List<Alumno>();

            foreach (Alumno alumno in alumnos)
            {
                // does this follow an order? wwat!
                alumno.Asistencias = alumno.Asistencias.Where(a => a.Semana.Calendario.CalendarioID == GetCurrentCalendario().CalendarioID).ToList();
                alumnosFiltrados.Add(alumno);
            }

            var model = new AsistenciasViewModel()
            {
                Alumnos = alumnosFiltrados
            };

            return View(model);
        }

        //
        // GET: /Profesor/Notas/
        public ActionResult Notas()
        {
            List<Alumno> alumnos = GetAlumnosProyecto();

            List<AlumnoNotaViewModel> model = new List<AlumnoNotaViewModel>();

            foreach (Alumno alumno in alumnos)
            {
                var nota = alumno.Notas.Where(n => n.Calendario.CalendarioID == GetCurrentCalendario().CalendarioID).SingleOrDefault();

                AlumnoNotaViewModel a = new AlumnoNotaViewModel()
                {
                    Nombre = alumno.Nombre,
                    Apellido = alumno.Apellido
                };

                if (nota != null)
                {
                    NotaViewModel alnota = new NotaViewModel()
                    {
                        Nota_Asistencia = ((float)nota.Nota_Asistencia).ToString("0.00"),
                        Nota_Minutas = ((float)nota.Nota_Minutas).ToString("0.00"),
                        Nota_EvaluacionFinal = ((float)nota.Nota_EvaluacionFinal).ToString("0.00"),
                        Nota_Final = ((float)nota.Nota_Final).ToString("0.00")
                    };

                    a.Nota = alnota;
                }
                else
                {
                    a.Nota = new NotaViewModel { Nota_Asistencia = "", Nota_EvaluacionFinal = "", Nota_Final = "", Nota_Minutas = "" };
                }

                model.Add(a);
            }

            return View(model);
        }

        //
        // GET: /Profesor/ListaAlumnos/
        public ActionResult ListaAlumnos()
        {
            List<Alumno> model = GetCurrentProyecto().Celulas.SelectMany(c => c.Alumnos).ToList();

            return View(model);
        }

        //
        // GET: /Profesor/EditarAlumno/2
        public ActionResult EditarAlumno(int? id)
        {
            if(id == null)
            {
                TempData["message"] = "No se envio ningún id";
                return RedirectToAction("ListaAlumnos");
            }

            var alumno = GetCurrentProyecto().Celulas.SelectMany(c => c.Alumnos).Where(a => a.AlumnoID == id).SingleOrDefault();

            if(alumno == null)
            {
                TempData["message"] = "No se encontro el alumno";
                return RedirectToAction("ListaAlumnos");
            }

            return View(new EditarAlumnoViewModel()
            {
                Celulas = new SelectList(GetCelulasList(), "Value", "Text", alumno.Celula.CelulaID.ToString()),
                alumnoID = alumno.AlumnoID
            });
        }

        //
        // POST: /Profesor/EditarAlumno/2
        [HttpPost]
        public ActionResult EditarAlumno(EditarAlumnoViewModel model)
        {
            if(ModelState.IsValid)
            {
                var alumno = _uow.RepositorioAlumnos.SelectById(model.alumnoID);

                if(alumno == null)
                {
                    TempData["message"] = "No se encontro el alumno";
                    return RedirectToAction("ListaAlumnos");
                }

                var celula = GetCurrentProyecto().Celulas.Where(c => c.CelulaID == int.Parse(model.CelulaID)).SingleOrDefault();

                if (celula == null)
                {
                    TempData["message"] = "No se encontro la celula";
                    return RedirectToAction("ListaAlumnos");
                }

                alumno.Celula = celula;
                _uow.RepositorioAlumnos.Update(alumno);
                _uow.Save();

                TempData["message"] = "El alumno fue guardado correctamente.";
                return RedirectToAction("ListaAlumnos");
            }

            return View(model.Celulas = new SelectList(GetCelulasList(), "Value", "Text"));
        }

        // Obtiene el usuario logueado actualmente
        private ApplicationUser GetCurrentUser()
        {
            // Tira una excepcion cuando el explorador ya tiene una sesion iniciada, debido a que al ejecutar el Seed, el id es totalmente distinto
            return _uow.RepositorioUsuarios.SelectAll().Where(u => u.Id == User.Identity.GetUserId()).SingleOrDefault();
        }

        private Proyecto GetCurrentProyecto()
        {
            return GetCurrentUser().Proyecto;
        }

        private Calendario GetCurrentCalendario()
        {
            var proyecto = GetCurrentProyecto();
            return proyecto.Calendarios.Where(c => c.CalendarioID == proyecto.CalendarioActualID).SingleOrDefault();
        }

        private Semana GetCurrentSemana()
        {
            var calendario = GetCurrentCalendario();
            return calendario.Semanas.Where(s => s.SemanaID == calendario.SemanaActualID).Single();
        }

        private string GetCoordinadorRoleID()
        {
            return _uow.RepositorioRoles.SelectAll().Where(r => r.Name == "Coordinador").Select(s => s.Id).Single();
        }

        private List<ApplicationUser> GetCoordinadores()
        {
            var proyecto = GetCurrentProyecto();
            return proyecto.Profesores.Where(u => u.Roles.Select(x => x.RoleId).Contains(GetCoordinadorRoleID())).ToList();
        }

        private List<SelectListItem> GetCelulasList()
        {
            var proyecto = GetCurrentProyecto();

            List<SelectListItem> items = proyecto.Celulas.Where(c => c.Proyecto.ProyectoID == proyecto.ProyectoID).Select(c => new SelectListItem() { Value = c.CelulaID.ToString(), Text = c.Nombre }).ToList();

            return items;
        }

        private List<SelectListItem> GetCoordinadoresList()
        {
            List<ApplicationUser> users = GetCoordinadores();
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (ApplicationUser u in users)
            {
                items.Add(new SelectListItem()
                {
                    Text = u.Nombre + " " + u.Apellido,
                    Value = u.Id
                });
            }

            return items;
        }

        private List<Alumno> GetAlumnosProyecto()
        {
            return GetCurrentProyecto().Celulas.SelectMany(c => c.Alumnos).ToList();
        }

        private List<SelectListItem> GetCoordinadoresLibresList()
        {
            List<ApplicationUser> users = GetCoordinadores();

            List<ApplicationUser> libres = users.Where(x => x.Celula == null).ToList();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (ApplicationUser u in libres)
            {
                items.Add(new SelectListItem()
                {
                    Text = u.Nombre + " " + u.Apellido,
                    Value = u.Id
                });
            }

            return items;
        }

    }
}