using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_notas_cit.Helpers
{
    public class ModelHelpers
    {
        public static String LimpiarPuntos(String cedula)
        {
            return cedula.Replace(".", "");
        }
    }
}