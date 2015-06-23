namespace Papyrus.Tests
{
    using System.Data.SqlClient;
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
            throw new System.NotImplementedException();
        }

        public void Update(Document document)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string documentId)
        {
            throw new System.NotImplementedException();
        }
    }
}