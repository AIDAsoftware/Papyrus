namespace Papyrus.Web.Controllers
{
    using System.Web.Mvc;

    public class DocumentsController : Controller
    {
        public ActionResult Detail(string id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}