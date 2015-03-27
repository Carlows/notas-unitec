using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace control_notas_cit.Models.Entidades
{
    public class Calendario
    {
        public Calendario()
        {
            this.Finalizado = false;
            this.IsLastWeek = false;
        }

        [Key]
        public int CalendarioID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int SemanaActualID { get; set; }
        public bool? IsLastWeek { get; set; }
        public bool? Finalizado { get; set; }

        // Notas
        public float? Notas_Minutas_Valor { get; set; }
        public float? Notas_Asistencias_Valor { get; set; }
        public float? Notas_Evaluacion_Final_Valor { get; set; }

        public virtual Proyecto Proyecto { get; set; }
        public virtual List<Semana> Semanas { get; set; }
        public virtual List<Nota> Notas { get; set; }
    }
}