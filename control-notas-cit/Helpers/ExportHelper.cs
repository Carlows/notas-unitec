using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace control_notas_cit.Helpers
{
    public class DataToConvert
    {
        public List<string> headers { get; set; }
        public List<List<string>> dataLines { get; set; }
    }

    public class ExportHelper
    {
        private static string DataLine(List<string> line)
        {
            string dataline = "";
            int count = 0;
            foreach (string header in line)
            {
                if (count > 0)
                    dataline += string.Format(",\"{0}\"", header);
                else
                    dataline += string.Format("\"{0}\"", header);
                count++;
            }

            return dataline;
        }
        
        /*
         * HEADER1, HEADER2, HEADER3
         * DATALINE1,DATALINE2,DATALINE3,
         * DATALINE1,DATALINE2,DATALINE3,         
         * */
        public static String ConvertToCSV(DataToConvert datos)
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine(DataLine(datos.headers));

            foreach(List<string> line in datos.dataLines)
            {
                sw.WriteLine(DataLine(line));
            }

            return sw.ToString();
        }

        public static String ConvertToExcel<T>(List<T> datos)
        {
            var grid = new GridView();
            grid.DataSource = datos;
            grid.DataBind();
            
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            return sw.ToString();
        }
    }
}