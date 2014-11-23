using control_notas_cit.Models.Entidades;
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace control_notas_cit.Models.ViewModels
{
    public class ProfesorIndexViewModel
    {
        public string NombreProfesor { get; set; }
        public Proyecto Proyecto { get; set; }
        public List<String> Profesores { get; set; }
        public Calendario Calendario { get; set; }
    }
    
    public class CalendarioViewModel
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name="Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [Required]
        public List<Semana> Semanas { get; set; }
        [Required]
        [Display(Name = "Valor minutas")]
        [Range(minimum: 0, maximum: 10, ErrorMessage="El valor debe estar entre 0 y 10")]
        public float? Notas_Minutas_Valor { get; set; }
        [Required]
        [Display(Name = "Valor asistencias")]
        [Range(minimum: 0, maximum: 10, ErrorMessage = "El valor debe estar entre 0 y 10")]
        public float? Notas_Asistencias_Valor { get; set; }
        [Required]
        [Display(Name = "Valor evaluacion final")]
        [Range(minimum: 0, maximum: 10, ErrorMessage = "El valor debe estar entre 0 y 10")]
        public float? Notas_Presentacion_Valor { get; set; }
    }

    public class SemanaViewModel
    {
        [Required]
        public int SemanaID { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.MultilineText)]
        public string Actividad { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Fecha { get; set; }
        public int NumeroSemana { get; set; }
    }

    public class FinalizarSemanaViewModel
    {
        public Semana Semana { get; set; }
        public List<Minuta> MinutasPorAprobar { get; set; }
    }

    public class CelulaViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name="Coordinadores")]
        public List<string> CoordinadoresID { get; set; }

        public MultiSelectList Coordinadores { get; set; }
    }

    public class CelulaEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
    }

    public class CoordinadorViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage="El email es requerido.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es requerido.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "La cedula es requerida.")]
        public string Cedula { get; set; }
        [Required(ErrorMessage="La contraseña es requerida.")]
        [Display(Name="Contraseña")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [Display(Name = "Repetir Contraseña")]
        public string ConfirmarPassword { get; set; }
        [Required(ErrorMessage="El telefono es requerido.")]
        public string Telefono { get; set; }

        public string Celula { get; set; }

        public SelectList Celulas { get; set; }
    }

    public class CoordinadorEditViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es requerido.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "La cedula es requerida.")]
        public string Cedula { get; set; }
        [Required(ErrorMessage = "El telefono es requerido.")]
        public string Telefono { get; set; }
        [Display(Name="Celula")]
        public string CelulaID { get; set; }

        public SelectList Celulas { get; set; }
    }

    public class MinutaPartialViewModel
    {
        public Semana CurrentSemana { get; set; }
        public List<Minuta> Minutas { get; set; }
    }

    public class AsistenciasViewModel
    {
        public List<Alumno> Alumnos { get; set; }
    }

    public class CargarNotasViewModel
    {
        public List<Alumno> Alumnos { get; set; }
        [Required]
        public List<ID_Alumno_Nota> Notas { get; set; }
    }

    public class ID_Alumno_Nota
    {
        public int ID { get; set; }
        [Required]
        [Integer(ErrorMessage = "El valor debe ser un numero entero")]
        [Range(0, 4, ErrorMessage = "Solo puedes introducir un valor del 0 al 4")]
        public int Nota { get; set; }
    }

    public class AlumnoNotaViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public NotaViewModel Nota { get; set; }
    }

    public class NotaViewModel
    {
        public string Nota_Asistencia { get; set; }
        public string Nota_Minutas { get; set; }
        public string Nota_EvaluacionFinal { get; set; }
        public string Nota_Final { get; set; }
    }

    public class EditarAlumnoViewModel
    {
        public SelectList Celulas { get; set; }

        [Required]
        public int alumnoID { get; set; }

        [Required(ErrorMessage = "Debes elegir una celula")]
        [Display(Name="Celula")]
        public string CelulaID { get; set; }
    }
}