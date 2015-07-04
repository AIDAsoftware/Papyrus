namespace Papyrus.Business.Documents
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;

    public class SqlDocumentRepository : DocumentRepository
    {
        private readonly string connectionString;

        public SqlDocumentRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionForTests"].ConnectionString;
        }

        public async Task Save(Document document)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                const string insertSqlQuery = @"INSERT Documents(Id, Title, Description, Content, Language) 
                                                VALUES (@id, @title, @desc, @content, @lang);";
                await connection.ExecuteAsync(insertSqlQuery, new {
                    id = document.Id,
                    title = document.Title,
                    desc = document.Description,
                    content = document.Content,
                    lang = document.Language,
                });
            }
            
        }

        public async Task<Document> GetDocument(string id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                //TODO: explicit select
                const string selectSqlQuery = @"SELECT *" + "FROM [Documents]" + "WHERE Id = @Id;";
                return (await connection.QueryAsync<Document>(selectSqlQuery, new {Id = id})).First();
            }
        }

        public async Task Update(Document document)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                const string updateSqlQuery = @"UPDATE Documents " + 
                                              "SET Title = @Title, " + 
                                              "Description = @Description, " + 
                                              "Content = @Content, " + 
                                              "Language = @Language " + 
                                              "WHERE Id = @Id;";
                var affectedRows = await connection.ExecuteAsync(updateSqlQuery, new
                {
                    Id = document.Id,
                    Title = document.Title,
                    Description = document.Description,
                    Content = document.Content,
                    Language = document.Language
                });
                if (affectedRows == 0) throw new DocumentNotFoundException();
            }
        }

        public async Task Delete(string documentId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                const string deleteSqlQuery = @"DELETE FROM Documents WHERE Id = @Id";
                await connection.ExecuteAsync(deleteSqlQuery, new
                {
                    Id = documentId,
                });
            }
        }

        //TODO: devolver IEnumerable ??
        public async Task<List<Document>> GetAllDocuments()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                const string selectAllDocumentsSqlQuery = @"SELECT * FROM Documents";
                return (await connection.QueryAsync<Document>(selectAllDocumentsSqlQuery)).ToList();
            }
        }
    }
}