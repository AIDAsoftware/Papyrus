using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Papyrus.Business;

namespace Papyrus.Api.Controllers
{
    public class DocumentationController : ApiController
    {
        private readonly InMemoryDocumentsRepository documentsRepository = new InMemoryDocumentsRepository();

        [Route("documentation/products/{productId}/versions/{versionId}"), HttpGet]
        public List<Document> GetDocumentationFor(string productId, string versionId) {
            return new GetDocumentation(documentsRepository).ExecuteFor(productId, versionId);            
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
            throw new NotImplementedException();
        }
    }
}
