
namespace Papyrus.Business.Documents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Infrastructure.Core.Database;

    public class SqlDocumentRepository : DocumentRepository
    {
        private readonly DatabaseConnection connection;

        public SqlDocumentRepository(DatabaseConnection connection) {
            this.connection = connection;
        }

        public async Task Save(Document document)
        {
            const string insertSqlQuery = @"INSERT Documents(Id, ProductId, ProductVersionId, Language, Title, Description, Content) " +
                                            "VALUES (@Id, @ProductId, @ProductVersionId, @Language, @Title, @Description, @Content);";
            await connection.Execute(insertSqlQuery, new {
                Id = document.DocumentIdentity.Id,
                ProductId = document.DocumentIdentity.ProductId,
                ProductVersionId = document.DocumentIdentity.VersionId,
                Language = document.DocumentIdentity.Language,
                Title = document.Title,
                Description = document.Description,
                Content = document.Content
            });
        }

        public async Task<Document> GetDocument(string id)
        {
            const string selectSqlQuery = @"SELECT Id, ProductId, ProductVersionId, Title, Content, Description, Language
                                            FROM [Documents] WHERE Id = @Id;";
            var result = (await connection.Query<dynamic>(selectSqlQuery, new {Id = id})).FirstOrDefault();

            if (result == null) return null;

            return new Document()
                .WithId(result.Id)
                .WithTitle(result.Title)
                .WithContent(result.Content)
                .WithDescription(result.Description)
                .ForLanguage(result.Language)
                .ForProduct(result.ProductId)
                .ForProductVersion(result.ProductVersionId);
        }

        public async Task Update(Document document)
        {
            const string updateSqlQuery = @"UPDATE Documents " + 
                                            "SET Title = @Title, " + 
                                            "Description = @Description, " + 
                                            "Content = @Content " + 
                                            "WHERE Id = @Id AND ProductId = @ProductId " +
                                                            "AND ProductVersionId = @ProductVersionId " +
                                                            "AND Language = @Language;";
            var affectedRows = await connection.Execute(updateSqlQuery, new
            {
                Id = document.DocumentIdentity.Id,
                ProductId = document.DocumentIdentity.ProductId,
                ProductVersionId = document.DocumentIdentity.VersionId,
                Language = document.DocumentIdentity.Language,
                Title = document.Title,
                Description = document.Description,
                Content = document.Content
            });
        }

        public async Task Delete(string documentId)
        {
            const string deleteSqlQuery = @"DELETE FROM Documents WHERE Id = @Id";
            var affectedRows = await connection.Execute(deleteSqlQuery, new
            {
                Id = documentId,
            });
        }

        //TODO: devolver IEnumerable ??
        public async Task<List<Document>> GetAllDocuments()
        {
            const string selectAllDocumentsSqlQuery = @"SELECT Id, ProductId, ProductVersionId, Title, Content, Description, Language
                                                        FROM Documents;";
            var result = (await connection.Query<dynamic>(selectAllDocumentsSqlQuery)).ToList();

            var documents = new List<Document>();
            result.ForEach(document => 
                documents.Add(new Document().WithId(document.Id)
                                  .ForProduct(document.ProductId)
                                  .ForProductVersion(document.ProductVersionId)
                                  .ForLanguage(document.Language)
                                  .WithTitle(document.Title)
                                  .WithContent(document.Content)
                                  .WithDescription(document.Description)));
            return documents;
        }
    }
}