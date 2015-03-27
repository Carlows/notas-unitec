using control_notas_cit.Models.Entidades;
using control_notas_cit.Models.Repositorios;
using IdentitySample.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Servicios
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationDbContext _context;
        private IRepositorioGenerico<Celula> repoCelulas;
        private IRepositorioGenerico<ApplicationUser> repoUsers;
        private IRepositorioGenerico<Proyecto> repoProyectos;
        private IRepositorioGenerico<Calendario> repoCalendarios;
        private IRepositorioGenerico<Semana> repoSemanas;
        private IRepositorioGenerico<IdentityRole> repoRoles;
        private IRepositorioGenerico<Alumno> repoAlumnos;
        private IRepositorioGenerico<Minuta> repoMinutas;
        private IRepositorioGenerico<Asistencia> repoAsistencias;
        private IRepositorioGenerico<Nota> repoNotas;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepositorioGenerico<ApplicationUser> RepositorioUsuarios
        {
            get
            {
                if (this.repoUsers == null)
                {
                    this.repoUsers = new RepositorioGenerico<ApplicationUser>(_context);
                }

                return repoUsers;
            }
        }

        public IRepositorioGenerico<IdentityRole> RepositorioRoles
        {
            get
            {
                if (this.repoRoles == null)
                {
                    this.repoRoles = new RepositorioGenerico<IdentityRole>(_context);
                }

                return repoRoles;
            }
        }

        public IRepositorioGenerico<Calendario> RepositorioCalendarios
        {
            get
            {
                if (this.repoCalendarios == null)
                {
                    this.repoCalendarios = new RepositorioGenerico<Calendario>(_context);
                }

                return repoCalendarios;
            }
        }

        public IRepositorioGenerico<Proyecto> RepositorioProyectos
        {
            get
            {
                if (this.repoProyectos == null)
                {
                    this.repoProyectos = new RepositorioGenerico<Proyecto>(_context);
                }

                return repoProyectos;
            }
        }

        public IRepositorioGenerico<Celula> RepositorioCelulas
        {
            get
            {

                if (this.repoCelulas == null)
                {
                    this.repoCelulas = new RepositorioGenerico<Celula>(_context);
                }

                return repoCelulas;
            }
        }

        public IRepositorioGenerico<Nota> RepositorioNotas
        {
            get
            {
                if (this.repoNotas == null)
                {
                    this.repoNotas = new RepositorioGenerico<Nota>(_context);
                }

                return repoNotas;
            }
        }

        public IRepositorioGenerico<Asistencia> RepositorioAsistencias
        {
            get
            {
                if (this.repoAsistencias == null)
                {
                    this.repoAsistencias = new RepositorioGenerico<Asistencia>(_context);
                }

                return repoAsistencias;
            }
        }

        public IRepositorioGenerico<Semana> RepositorioSemanas
        {
            get
            {
                if (this.repoSemanas == null)
                {
                    this.repoSemanas = new RepositorioGenerico<Semana>(_context);
                }

                return repoSemanas;
            }
        }

        public IRepositorioGenerico<Minuta> RepositorioMinutas
        {
            get
            {
                if (this.repoMinutas == null)
                {
                    this.repoMinutas = new RepositorioGenerico<Minuta>(_context);
                }

                return repoMinutas;
            }
        }

        public IRepositorioGenerico<Alumno> RepositorioAlumnos
        {
            get
            {
                if (this.repoAlumnos == null)
                {
                    this.repoAlumnos = new RepositorioGenerico<Alumno>(_context);
                }

                return repoAlumnos;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}