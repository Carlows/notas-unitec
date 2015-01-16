using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using control_notas_cit.Models.Entidades;
using Microsoft.AspNet.Identity.Owin;
using control_notas_cit.Models.Repositorios;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;

namespace control_notas_cit.Helpers
{
    public static class RoleType
    {
        public const string Administrador = "Admin";
        public const string Profesor = "Profesor";
        public const string Coordinador = "Coordinador";
    }

    public class HelperGetterMethods
    {
    }
}