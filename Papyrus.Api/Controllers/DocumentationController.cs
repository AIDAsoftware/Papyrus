using System;
using System.Collections.Generic;
using System.Web.Http;
using Papyrus.Business;

namespace Papyrus.Api.Controllers
{
    public class DocumentationController : ApiController
    {
        private static readonly InMemoryDocumentsRepository documentsRepository = new InMemoryDocumentsRepository();

        [Route("products/{productId}/versions/{versionId}/documents"), HttpGet]
        public List<Document> GetDocumentationFor(string productId, string versionId) {
            return new GetDocumentation(documentsRepository).ExecuteFor(productId, versionId);            
        }

        [Route("products/{productId}/versions/{versionId}/documents"), HttpPost]
        public void CreateDocument(string productId, string versionId, [FromBody]DocumentDto documentDto)
        {
            new CreateDocument(documentsRepository).ExecuteFor(documentDto, productId, versionId);
        }
    }

    internal class InMemoryDocumentsRepository : DocumentsRepository {
        private Dictionary<string, Documentation> documentations = 
                                    new Dictionary<string, Documentation>();

        public Documentation GetDocumentationFor(string productId, string versionId) {
            if (!documentations.ContainsKey(productId + versionId))
                return Documentation.WithDocuments(new List<Document>());
            return documentations[productId + versionId];
        }

        public void CreateDocumentFor(Document document, string productId, string versionId) {
            if (!documentations.ContainsKey(productId + versionId)) {
                documentations.Add(productId + versionId, Documentation.WithDocuments(new List<Document>()));
            }
            documentations[productId + versionId].Documents.Add(document);
        }
    }
}
