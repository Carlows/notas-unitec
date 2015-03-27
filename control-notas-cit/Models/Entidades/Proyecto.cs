using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Entidades
{
    public class Proyecto
    {
        [Key]
        public int ProyectoID { get; set; }
        public string Nombre { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        public int CalendarioActualID { get; set; }

        public virtual List<ApplicationUser> Profesores { get; set; }
        public virtual List<Celula> Celulas { get; set; }
        public virtual List<Calendario> Calendarios { get; set; }
    }
}