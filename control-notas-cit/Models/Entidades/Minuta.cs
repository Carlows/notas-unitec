using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Entidades
{
    public class Minuta
    {
        public Minuta()
        {
            this.Aprobada = false;
            this.Fecha = DateTime.Now;
        }

        [Key]
        public int MinutaID { get; set; }
        public DateTime Fecha { get; set; }
        public bool? Aprobada { get; set; }

        [DataType(DataType.MultilineText)]
        public string Contenido { get; set; }

        public virtual Celula Celula { get; set; }
        public virtual Semana Semana { get; set; }
    }
}