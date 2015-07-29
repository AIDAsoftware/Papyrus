namespace Papyrus.WebServices.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Business.Documents;
    using Business.Documents.Exceptions;
    using Models;

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

            return DocumentDtoFrom(document);
        }

        public async Task<DocumentDto[]> Get()
        {
            var documents = await documentService.AllDocuments();
            return documents.Select(document => DocumentDtoFrom(document)).ToArray();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Add([FromBody] DocumentDto documentDto)
        {
            var document = DocumentFrom(documentDto);
            await documentService.Create(document);
            return new HttpResponseMessage(HttpStatusCode.Created);

        }

        [HttpPut]
        public async Task<HttpResponseMessage> Update(string id, [FromBody] DocumentDto documentDto)
        {
            var document = DocumentFrom(documentDto).WithId(id);
            try
            {
                await documentService.Update(document);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            catch (DocumentIdCouldBeDefinedException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        private static Document DocumentFrom(DocumentDto documentDto)
        {
            return new Document()
                .WithTitle(documentDto.Title)
                .WithContent(documentDto.Content)
                .WithDescription(documentDto.Description)
                .ForLanguage(documentDto.Language);
        }

        private static DocumentDto DocumentDtoFrom(Document document)
        {
            return new DocumentDto()
            {
                Id = document.Id,
                Title = document.Title,
                Description = document.Description,
                Content = document.Content,
                Language = document.Language
            };
        }
    }
}