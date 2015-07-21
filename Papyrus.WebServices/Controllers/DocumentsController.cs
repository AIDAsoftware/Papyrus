using Papyrus.WebServices.Models;

namespace Papyrus.WebServices.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Business.Documents;
    using Business.Documents.Exceptions;

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

        public async Task<DocumentDto> Get(string id)
        {
            var document = await documentService.GetDocumentById(id);
            if (document == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new DocumentDto()
            {
                Title = document.Title,
                Description = document.Description,
                Content = document.Content,
                Language = document.Language
            };
        }
         
    }
}