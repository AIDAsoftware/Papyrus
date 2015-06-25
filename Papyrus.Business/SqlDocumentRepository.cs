namespace Papyrus.Tests
{
    using System.Data.SqlClient;
    using System.Linq;
    using Business;
    using Dapper;

    public class SqlDocumentRepository : DocumentRepository
    {
        public void Save(Document document)
        {
            using (var connection = new SqlConnection(@"server=.\SQLExpress;database=Papyrus;trusted_connection = true"))
            {
                connection.Open();
                connection.Execute(@"INSERT Documents(Id, Title, Description, Content, Language) 
                                VALUES (@id, @title, @desc, @content, @lang);", 
                                new { id = document.Id, title = document.Title, desc = document.Description,
                                    content = document.Content, lang = document.Language,
                                });
            }
            
        }

        public Document GetDocument(string id)
        {
            using (var connection = new SqlConnection(@"server=.\SQLExpress;database=Papyrus;trusted_connection = true"))
            {
                return connection.Query<Document>(@"SELECT *" +
                                           "FROM [Documents]" +
                                           "WHERE Id = @Id;", new {Id = "AnyId"})
                                           .First();
            }
        }

        public void Update(Document document)
        {
            using (var connection = new SqlConnection(@"server=.\SQLExpress;database=Papyrus;trusted_connection = true"))
            {
                connection.Query<Document>(@"UPDATE Documents " +
                                           "SET Title = @Title, " +
                                                "Description = @Description, " +
                                                "Content = @Content, " +
                                                "Language = @Language " +
                                           "WHERE Id = @Id;", 
                                           new {
                                               Id = document.Id,
                                               Title = document.Title,
                                               Description = document.Description,
                                               Content = document.Content,
                                               Language = document.Language
                                           });
            }
        }

        public void Delete(string documentId)
        {
            throw new System.NotImplementedException();
        }
    }
}