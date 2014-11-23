using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace control_notas_cit.Helpers
{
    public static class HelperMethods
    {
        public static MvcHtmlString IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return new MvcHtmlString(controller == currentController && action == currentAction ? cssClass : String.Empty);
        }

        public static MvcHtmlString TruncateText(this HtmlHelper html, string texto, int length)
        {
            if (texto == null)
            {
                return new MvcHtmlString(String.Empty);
            }

            if (texto.Length > length)
            {
                texto = texto.Substring(0, length) + "...";
            }

            return new MvcHtmlString(texto);
        }

        public static MvcHtmlString GridBoxesOpen(this HtmlHelper html, int counter)
        {
            String output = null;
            if (counter == 0)
            {
                output = String.Format("<div class='row-fluid'>");
            }
            else if (counter > 0 && counter % 2 == 0)
            {
                output = String.Format("</div><div class=\"row-fluid\">");
            }

            return new MvcHtmlString(output);
        }

        public static MvcHtmlString GridBoxesClose(this HtmlHelper html, int counter, int length)
        {
            String output = null;
            if(counter == (length - 1))
            {
               output = String.Format("</div>");
            }

            return new MvcHtmlString(output);
        }
    }
}