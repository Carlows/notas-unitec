using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    
    public class Asistencia
    {
        public Asistencia()
        {
            // Revisar
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