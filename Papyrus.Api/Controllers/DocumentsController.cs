using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using Papyrus.Business.Actions;
using Papyrus.Business.Domain.Documents;
using Papyrus.Infrastructure.Core;
using Papyrus.Infrastructure.Repositories;

namespace Papyrus.Api.Controllers
{
    public class DocumentsController : ApiController {
        private static readonly string DocumentsPath =
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MyDocuments"));
        private static readonly DocumentsRepository documentsRepository = new FileDocumentsRepository(new JsonFileSystemProvider(DocumentsPath));

        [Route("products/{productId}/versions/{versionId}/documents"), HttpGet]
        public List<Document> GetDocumentationFor(string productId, string versionId) {
            return new GetDocumentation(documentsRepository).ExecuteFor(productId, versionId).ToList();            
        }

        [Route("products/{productId}/versions/{versionId}/documents"), HttpPost]
        public void CreateDocument(string productId, string versionId, [FromBody]DocumentDto documentDto) {
            documentDto.ProductId = productId;
            documentDto.VersionId = versionId;
            new CreateDocument(documentsRepository).ExecuteFor(documentDto);
        }
    }
}
