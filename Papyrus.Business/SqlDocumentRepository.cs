namespace Papyrus.Tests
{
    using System.Data.SqlClient;
    using Business;
    using Dapper;

    public class SqlDocumentRepository : DocumentRepository
    {
        public void Save(Document document)
        {
            var connection = new SqlConnection(@"server=.\SQLExpress;database=Papyrus;trusted_connection = true");
            connection.Open();
            connection.Execute(@"INSERT Documents(Id, Title, Description, Content, Language) 
                                VALUES (@id, @t, @d, @c, @l);", 
                new
                {
                    id = document.Id,
                    t = document.Title,
                    d = document.Description,
                    c = document.Content,
                    l = document.Language,
                });
            connection.Close();
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