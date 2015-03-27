using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Entidades
{
    public class Alumno
    {
        [Key]
        public int AlumnoID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public virtual Celula Celula { get; set; }
        public virtual List<Nota> Notas { get; set; }
        public virtual List<Asistencia> Asistencias { get; set; }
    }
}