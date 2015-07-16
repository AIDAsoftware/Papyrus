namespace Papyrus.Web.Controllers
{
    using System.Web.Mvc;
    using System.Web.UI.WebControls;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        } 
    }
}