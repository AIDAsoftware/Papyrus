using Papyrus.WebServices.Models;

namespace Papyrus.WebServices.Controllers
{
    using System;
    using System.Linq;
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

        public async Task<DocumentDto[]> Get()
        {
            var documents = await documentService.AllDocuments();

            return documents.Select(document => new DocumentDto
            {
                Title = document.Title,
                Description = document.Description,
                Content = document.Content,
                Language = document.Language
            }).ToArray();
        }


    }
}