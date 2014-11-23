using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Repositorios
{
    public interface IRepositorioGenerico<T> where T : class
    {
        IEnumerable<T> SelectAll();
        T SelectById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}