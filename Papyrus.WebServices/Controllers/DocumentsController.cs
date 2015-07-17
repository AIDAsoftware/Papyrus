using Papyrus.WebServices.Models;

namespace Papyrus.WebServices.Controllers
{
    using System.Web.Http;
    using Business.Documents;

    public class DocumentsController : ApiController {
        private readonly DocumentService documentService;
        public DocumentsController(DocumentService documentService) {
            this.documentService = documentService;
        }

        [HttpPost]
        public string Add([FromBody] DocumentDto document)
        {
            documentService.Create(
                new Document()
                    .WithContent(document.Content)
                    .WithDescription(document.Description)
                    .WithTitle(document.Title)
                    .ForLanguage(document.Language)
                );
            return "{ Message: Document created }";
        }

        public DocumentDto Get()
        {
            return new DocumentDto() {Title="Any"};
        }
         
    }
}