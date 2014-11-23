using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Repositorios
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        private ApplicationDbContext contexto;
        private DbSet<T> tabla = null;

        public RepositorioGenerico(ApplicationDbContext context)
        {
            this.contexto = context;
            tabla = contexto.Set<T>();
        }

        public IEnumerable<T> SelectAll()
        {
            return tabla.ToList();
        }

        public T SelectById(object id)
        {
            return tabla.Find(id);
        }

        public void Insert(T obj)
        {
            tabla.Add(obj);
        }

        public void Update(T obj)
        {
            tabla.Attach(obj);
            contexto.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T obj = tabla.Find(id);
            tabla.Remove(obj);
        }

        public void Save()
        {
            contexto.SaveChanges();
        }
    }
} 