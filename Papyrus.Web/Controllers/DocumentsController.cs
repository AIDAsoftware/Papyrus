namespace Papyrus.Web.Controllers
{
    using System.Web.Mvc;

    public class DocumentsController : Controller
    {
        public ActionResult Details(string id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult AllDocuments()
        {
            return View();
        }
    }
}