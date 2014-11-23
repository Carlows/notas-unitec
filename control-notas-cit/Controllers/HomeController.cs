using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        /*
         * Este controlador solo funciona para redireccionar al usuario hacia el controlador especifico
         * TODO: Evitar que cuando el usuario ya este autorizado, este index se muestre.
         * */
        [Authorize]
        public ActionResult Index()
        {
            return RedirectToAction("RedirectToController", "Account");
        }
    }
}
