namespace Papyrus.Business
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;

    public class SqlDocumentRepository : DocumentRepository
    {
        private const string Server = @"server=.\SQLExpress;database=Papyrus;trusted_connection = true";

        private const string InsertSqlQuery = @"INSERT Documents(Id, Title, Description, Content, Language) 
                                                VALUES (@id, @title, @desc, @content, @lang);";

        private const string SelectSqlQuery = @"SELECT *" + "FROM [Documents]" + "WHERE Id = @Id;";

        private const string UpdateSqlQuery = @"UPDATE Documents " + 
                                                "SET Title = @Title, " + 
                                                    "Description = @Description, " + 
                                                    "Content = @Content, " + 
                                                    "Language = @Language " + 
                                                "WHERE Id = @Id;";

        private const string DeleteSqlQuery = @"DELETE FROM Documents WHERE Id = @Id";

        private const string AllDocumentsSqlQuery = @"SELECT * FROM Documents";


        public void Save(Document document)
        {
            using (var connection = new SqlConnection(Server))
            {
                connection.Open();
                connection.Execute(InsertSqlQuery, new {
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
            using (var connection = new SqlConnection(Server))
                return (await connection.QueryAsync<Document>(SelectSqlQuery, new {Id = id})).First();
        }

        public async Task Update(Document document)
        {
            using (var connection = new SqlConnection(Server))
            {
                var affectedRows = await connection.ExecuteAsync(UpdateSqlQuery, new
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
            using (var connection = new SqlConnection(Server))
                await connection.ExecuteAsync(DeleteSqlQuery, new
                {
                    Id = documentId,
                });
        }

        public IEnumerable<Document> GetAllDocuments()
        {
            using (var connection = new SqlConnection(Server))
                return connection.Query<Document>(AllDocumentsSqlQuery);
        }
    }
}