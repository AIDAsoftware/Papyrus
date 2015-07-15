namespace Papyrus.WebServices.Controllers
{
    using System.Web.Http;
    using System.Web.Http.Results;
    using Business.Documents;

    public class DocumentsController : ApiController
    {
        [HttpPost]
        public string Add([FromBody] DocumentDto document)
        {
            var documentService = new DocumentService(new SqlDocumentRepository());
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

    public class DocumentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }
    }
}