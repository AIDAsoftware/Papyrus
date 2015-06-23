namespace Papyrus.Tests
{
    using System.Data.SqlClient;
    using System.Linq;
    using Business;
    using Dapper;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SqlDocumentRepositoryShould
    {
        private SqlConnection connection;

        [SetUp]
        public void SetUp()
        {
            connection = new SqlConnection(
                @"server=.;database=Papyrus;trusted_connection = true"
                );
            connection.Open();
        }


        [TearDown]
        public void TearDown()
        {
            connection.Execute(@"DELETE FROM Documents WHERE Id = @Id;", new {Id = "AnyId"});
            connection.Close();
        }


        [Test]
        public void save_a_document()
        {
            var document = new Document()
                .WithId("AnyId");

            new SqlDocumentRepository().Save(document);

            var requestedDocuments = connection
                .Query<Document>(@"SELECT *" +
                                    "FROM [Documents]" +
                                    "WHERE Id = @Id;", new { Id = "AnyId" });
            requestedDocuments.First().ShouldBeEquivalentTo(document);
        }
    }
}