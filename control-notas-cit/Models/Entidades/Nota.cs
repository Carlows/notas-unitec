using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Entidades
{
    public class Nota
    {
        [Key]
        public int NotaID { get; set; }
        public float? Nota_Minutas { get; set; }
        public float? Nota_Asistencia { get; set; }
        public float? Nota_EvaluacionFinal { get; set; }
        public float? Nota_Final { get; set; }

        public virtual Calendario Calendario { get; set; }

        [Required]
        public virtual Alumno Alumno { get; set; }
    }
}