
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
            const string insertSqlQuery = @"INSERT Documents(Id, Title, Description, Content, Language) 
                                            VALUES (@id, @title, @desc, @content, @lang);";
            await connection.Execute(insertSqlQuery, new {
                id = document.Id,
                title = document.Title,
                desc = document.Description,
                content = document.Content,
                lang = document.Language,
            });
        }

        public async Task<Document> GetDocument(string id)
        {
            const string selectSqlQuery = @"SELECT Id, Title, Content, Description, Language
                                            FROM [Documents] WHERE Id = @Id;";
            return (await connection.Query<Document>(selectSqlQuery, new {Id = id})).First();
        }

        public async Task Update(Document document)
        {
            const string updateSqlQuery = @"UPDATE Documents " + 
                                            "SET Title = @Title, " + 
                                            "Description = @Description, " + 
                                            "Content = @Content, " + 
                                            "Language = @Language " + 
                                            "WHERE Id = @Id;";
            var affectedRows = await connection.Execute(updateSqlQuery, new
            {
                Id = document.Id,
                Title = document.Title,
                Description = document.Description,
                Content = document.Content,
                Language = document.Language
            });
            if (affectedRows == 0) throw new DocumentNotFoundException();
        }

        public async Task Delete(string documentId)
        {
            const string deleteSqlQuery = @"DELETE FROM Documents WHERE Id = @Id";
            await connection.Execute(deleteSqlQuery, new
            {
                Id = documentId,
            });
        }

        //TODO: devolver IEnumerable ??
        public async Task<List<Document>> GetAllDocuments()
        {
            const string selectAllDocumentsSqlQuery = @"SELECT Id, Title, Content, Description, Language
                                                        FROM Documents";
            return (await connection.Query<Document>(selectAllDocumentsSqlQuery)).ToList();
        }
    }
}