using System;
using System.Collections.Generic;
using System.Web.Http;
using Papyrus.Business;
using Papyrus.Business.Actions;
using Papyrus.Business.Domain.Documents;
using Papyrus.Business.Domain.Products;

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
        public void CreateDocument(string productId, string versionId, [FromBody]DocumentDto documentDto) {
            documentDto.ProductId = productId;
            documentDto.VersionId = versionId;
            new CreateDocument(documentsRepository).ExecuteFor(documentDto);
        }
    }

    internal class InMemoryDocumentsRepository : DocumentsRepository {
        private Dictionary<string, Documentation> documentations = 
                                    new Dictionary<string, Documentation>();

        public Documentation GetDocumentationFor(VersionIdentifier versionId) {
            return GetDocumentationFor(new VersionIdentifier(versionId.ProductId, versionId.VersionId));
        }

        public void CreateDocumentFor(Document document) {
            var productId = document.VersionIdentifier.ProductId;
            var versionId = document.VersionIdentifier.VersionId;
            if (!documentations.ContainsKey(productId + versionId)) {
                documentations.Add(productId + versionId, Documentation.WithDocuments(new List<Document>()));
            }
            documentations[productId + versionId].Documents.Add(document);
        }
    }
}
