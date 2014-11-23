using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using control_notas_cit.Models.Entidades;
using IdentitySample.Models;

namespace control_notas_cit.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Este campo es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        [Required]
        public IEnumerable<string> Profesores { get; set; }
    }

    public class ProjectEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
    }

    public class AdminViewModel
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
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [Display(Name = "Contraseña")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [Display(Name = "Repetir Contraseña")]
        public string ConfirmarPassword { get; set; }
        [Required(ErrorMessage = "El telefono es requerido.")]
        public string Telefono { get; set; }
    }

    public class ProfesorViewModel
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
        [Display(Name="Proyecto")]
        public int? ProyectoID { get; set; }

        public SelectList Proyectos { get; set; }
    }

    public class ProfesorEditViewModel
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
        [Display(Name="Proyecto")]
        public int? ProyectoID { get; set; }

        public SelectList Proyectos { get; set; }
    }

    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}