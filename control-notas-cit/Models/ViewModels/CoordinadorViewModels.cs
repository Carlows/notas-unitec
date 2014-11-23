using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using control_notas_cit.Models.Entidades;
using System.ComponentModel.DataAnnotations;

namespace control_notas_cit.Models.ViewModels
{
    public class CoordinadorIndexViewModel
    {
        public Celula Celula { get; set; }
        public Semana Semana { get; set; }
        public Minuta MinutaSemana { get; set; }
        public bool AsistenciaEnviada { get; set; }
    }

    public class AlumnoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Este campo es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Cedula { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class MinutaCelulaViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        public string Contenido { get; set; }
    }

    public class AsistenciaSemanaViewModel
    {
        public List<Alumno> Alumnos { get; set; }

        [Required]
        public List<int> ID_Alumnos { get; set; }
    }
}