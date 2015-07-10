namespace Papyrus.WebServices.Controllers
{
    using System.Web.Http;
    using Business.Documents;

    public class DocumentsController : ApiController
    {
        [HttpPost]
        public string Add(Document document)
        {
            var documentService = new DocumentService(new SqlDocumentRepository());
            documentService.Create(document);
            return "{ Message: Document created }";
        }

        public string Get()
        {
            return "Hello World!";
        }
         
    }
}