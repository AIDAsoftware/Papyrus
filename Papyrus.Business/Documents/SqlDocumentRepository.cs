
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
            const string insertSqlQuery = @"INSERT Documents(TopicId, ProductId, ProductVersionId, Language, Title, Description, Content) " +
                                            "VALUES (@TopicId, @ProductId, @ProductVersionId, @Language, @Title, @Description, @Content);";
            await connection.Execute(insertSqlQuery, new {
                TopicId = document.DocumentIdentity.TopicId,
                ProductId = document.DocumentIdentity.ProductId,
                ProductVersionId = document.DocumentIdentity.VersionId,
                Language = document.DocumentIdentity.Language,
                Title = document.Title,
                Description = document.Description,
                Content = document.Content
            });
        }

        public async Task<Document> GetDocument(string topicId)
        {
            const string selectSqlQuery = @"SELECT TopicId, ProductId, ProductVersionId, Title, Content, Description, Language
                                            FROM [Documents] WHERE TopicId = @TopicId;";
            var result = (await connection.Query<dynamic>(selectSqlQuery, new {TopicId = topicId})).FirstOrDefault();

            if (result == null) return null;

            return new Document()
                .WithTopicId(result.TopicId)
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
                                            "WHERE TopicId = @TopicId AND ProductId = @ProductId " +
                                                            "AND ProductVersionId = @ProductVersionId " +
                                                            "AND Language = @Language;";
            var affectedRows = await connection.Execute(updateSqlQuery, new
            {
                TopicId = document.DocumentIdentity.TopicId,
                ProductId = document.DocumentIdentity.ProductId,
                ProductVersionId = document.DocumentIdentity.VersionId,
                Language = document.DocumentIdentity.Language,
                Title = document.Title,
                Description = document.Description,
                Content = document.Content
            });
        }

        public async Task Delete(string topicId)
        {
            const string deleteSqlQuery = @"DELETE FROM Documents WHERE TopicId = @TopicId";
            var affectedRows = await connection.Execute(deleteSqlQuery, new
            {
                TopicId = topicId,
            });
        }

        //TODO: devolver IEnumerable ??
        public async Task<List<Document>> GetAllDocuments()
        {
            const string selectAllDocumentsSqlQuery = @"SELECT TopicId, ProductId, ProductVersionId, Title, Content, Description, Language
                                                        FROM Documents;";
            var result = (await connection.Query<dynamic>(selectAllDocumentsSqlQuery)).ToList();

            var documents = new List<Document>();
            result.ForEach(document => 
                documents.Add(new Document().WithTopicId(document.TopicId)
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