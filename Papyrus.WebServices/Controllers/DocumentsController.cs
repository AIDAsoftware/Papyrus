namespace Papyrus.WebServices.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Models;

    public class DocumentsController : ApiController {
        private readonly DocumentService documentService;

        public DocumentsController(DocumentService documentService) {
            this.documentService = documentService;
        }

        public async Task<DocumentDto> Get(string id)
        {
            var document = await documentService.GetDocumentByTopicId(id);
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
            var document = DocumentFrom(documentDto).WithTopicId(id);
            try
            {
                await documentService.Update(document);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            catch (DocumentNotFoundException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

//        [HttpDelete]
//        public async Task<HttpResponseMessage> Delete(string id)
//        {
//            try
//            {
//                await documentService.Remove(id);
//                return new HttpResponseMessage(HttpStatusCode.NoContent);
//            }
//            catch (DocumentNotFoundException)
//            {
//                return new HttpResponseMessage(HttpStatusCode.NotFound);
//            }
//        }

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
                Id = document.DocumentIdentity.TopicId,
                Title = document.Title,
                Description = document.Description,
                Content = document.Content,
                Language = document.DocumentIdentity.Language
            };
        }
    }
}