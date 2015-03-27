using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Entidades
{
    public class Semana
    {
        public Semana()
        {
            this.Iniciada = false;
            this.Finalizada = false;
        }

        [Key]
        public int SemanaID { get; set; }
        public int NumeroSemana { get; set; }
        [DataType(DataType.MultilineText)]
        public string Actividad { get; set; }
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        public bool Iniciada { get; set; }
        public bool Finalizada { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Fecha { get; set; }

        public virtual Calendario Calendario { get; set; }
        public virtual List<Minuta> Minutas { get; set; }
        public virtual List<Asistencia> Asistencias { get; set; }
    }
}