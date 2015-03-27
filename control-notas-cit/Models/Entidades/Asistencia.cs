using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Entidades
{
    public class Asistencia
    {
        public Asistencia()
        {
            this.Asistio = false;
        }

        [Key]
        public int AsistenciaID { get; set; }
        public bool? Asistio { get; set; }

        public virtual Celula Celula { get; set; }
        public virtual Semana Semana { get; set; }

        [Required]
        public virtual Alumno Alumno { get; set; }
    }
}