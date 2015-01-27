using control_notas_cit.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace control_notas_cit.Helpers
{
    public class ExportHelper
    {
        public static DataTable CreateDataTable(List<string> headers, List<DataAlumnoExport> alumnos)
        {
            DataTable dt = new DataTable();

            foreach(String h1 in headers)
            {
                dt.Columns.Add(h1);
            }

            foreach(DataAlumnoExport alumno in alumnos)
            {
                DataRow dr = dt.NewRow();

                int colcount = 0;
                for (int index = 0; index < alumno.GetType().GetProperties().Count(); index++)
                {
                    var value = alumno.GetType().GetProperties()[index].GetValue(alumno, null);

                    if(value != null)
                    {
                        dr[colcount] = value;
                        colcount++;
                    }
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable CreateAsistenciasDataTable(List<string> headers, List<DataAsistencias> data, int semanas)
        {
            DataTable dt = new DataTable();

            foreach(String h1 in headers)
            {
                dt.Columns.Add(h1);
            }

            foreach(DataAsistencias alumno in data)
            {
                DataRow dr = dt.NewRow();

                dr[0] = alumno.Nombre;
                dr[1] = alumno.Apellido;
                dr[2] = alumno.Cedula;
                dr[3] = alumno.Proyecto;

                int index = 4;
                foreach(Asistencia asistencia in alumno.Asistencias)
                {
                    dr[index] = (bool)asistencia.Asistio ? "asistio" : " ";
                    index++;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }
    }

    public class DataAsistencias
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Proyecto { get; set; }
        public List<Asistencia> Asistencias { get; set; }
    }

    public class DataAlumnoExport
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Proyecto { get; set; }
        public string Celula { get; set; }
        public float? Nota_Minutas { get; set; }
        public float? Nota_Asistencia { get; set; }
        public float? Nota_EvaluacionFinal { get; set; }
        public string Nota_Final { get; set; }
    }
}