using control_notas_cit.Models.Entidades;
using control_notas_cit.Models.Repositorios;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
namespace control_notas_cit.Models.Servicios
{
    public interface IUnitOfWork
    {
        IRepositorioGenerico<Alumno> RepositorioAlumnos { get; }
        IRepositorioGenerico<Asistencia> RepositorioAsistencias { get; }
        IRepositorioGenerico<Calendario> RepositorioCalendarios { get; }
        IRepositorioGenerico<Celula> RepositorioCelulas { get; }
        IRepositorioGenerico<Minuta> RepositorioMinutas { get; }
        IRepositorioGenerico<Nota> RepositorioNotas { get; }
        IRepositorioGenerico<Proyecto> RepositorioProyectos { get; }
        IRepositorioGenerico<IdentityRole> RepositorioRoles { get; }
        IRepositorioGenerico<Semana> RepositorioSemanas { get; }
        IRepositorioGenerico<ApplicationUser> RepositorioUsuarios { get; }
        void Save();
        void Dispose();
    }
}
