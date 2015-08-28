
namespace Papyrus.Business.Documents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exceptions;
    using Infrastructure.Core.Database;

    public class SqlDocumentRepository : DocumentRepository
    {
        private readonly DatabaseConnection connection;

        public SqlDocumentRepository(DatabaseConnection connection) {
            this.connection = connection;
        }

        public async Task Save(Document document)
        {
            const string insertSqlQuery = @"INSERT Documents(Id, ProductVersionId, Language, Title, Description, Content) 
                                            VALUES (@Id, @ProductVersionId, @Language, @Title, @Description, @Content);";
            await connection.Execute(insertSqlQuery, new {
                Id = document.Id,
                ProductVersionId = document.ProductVersionId,
                Language = document.Language,
                Title = document.Title,
                Description = document.Description,
                Content = document.Content
            });
        }

        public async Task<Document> GetDocument(string id)
        {
            const string selectSqlQuery = @"SELECT Id, ProductVersionId, Title, Content, Description, Language
                                            FROM [Documents] WHERE Id = @Id;";
            return (await connection.Query<Document>(selectSqlQuery, new {Id = id})).FirstOrDefault();
        }

        public async Task Update(Document document)
        {
            const string updateSqlQuery = @"UPDATE Documents " + 
                                            "SET Title = @Title, " + 
                                            "Description = @Description, " + 
                                            "Content = @Content " + 
                                            "WHERE Id = @Id AND ProductVersionId = @ProductVersionId AND Language = @Language;";
            var affectedRows = await connection.Execute(updateSqlQuery, new
            {
                Id = document.Id,
                ProductVersionId = document.ProductVersionId,
                Language = document.Language,
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
            const string selectAllDocumentsSqlQuery = @"SELECT Id, ProductVersionId, Title, Content, Description, Language
                                                        FROM Documents";
            return (await connection.Query<Document>(selectAllDocumentsSqlQuery)).ToList();
        }
    }
}