using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Entidades
{
    public class Celula
    {
        [Key]
        public int CelulaID { get; set; }
        public string Nombre { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        public virtual Proyecto Proyecto { get; set; }

        public virtual List<ApplicationUser> Coordinadores { get; set; }
        public virtual List<Alumno> Alumnos { get; set; }
        public virtual List<Minuta> Minutas { get; set; }
        public virtual List<Asistencia> Asistencias { get; set; }
    }
}