namespace Papyrus.Tests
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using Business;
    using Dapper;
    using FluentAssertions;
    using NSubstitute.Exceptions;
    using NUnit.Framework;

    [TestFixture]
    public class SqlDocumentRepositoryShould
    {
        private SqlConnection connection;

        [SetUp]
        public void SetUp()
        {
            connection = new SqlConnection(
                @"server=.\SQLExpress;database=Papyrus;trusted_connection = true"
                );
            connection.Open();
        }


        [TearDown]
        public void TearDown()
        {
            connection.Execute(@"DELETE FROM Documents");
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

        [Test]
        public void load_a_document()
        {
            connection.Execute(@"INSERT Documents(Id, Title, Description, Content, Language) 
                                VALUES (@id, NULL, NULL, NULL, NULL);",
                                new { id = "AnyId" });

            var document = new SqlDocumentRepository().GetDocument("AnyId");

            document.Id.Should().Be("AnyId");
            document.Title.Should().BeNull();
            document.Description.Should().BeNull();
            document.Content.Should().BeNull();
            document.Language.Should().BeNull();
        }

        [Test]
        public void update_a_document()
        {
            connection.Execute(@"INSERT Documents(Id, Title, Description, Content, Language) 
                                VALUES (@id, @title, NULL, NULL, NULL);",
                                new { id = "AnyId", title = "AnyTitle" });

            var document = new Document()
                .WithId("AnyId")
                .WithTitle("NewTitle")
                .WithDescription("AnyDescription");

            new SqlDocumentRepository().Update(document);
            document = connection
                .Query<Document>(@"SELECT *" +
                                    "FROM [Documents]" +
                                    "WHERE Id = @Id;", new { Id = "AnyId" }).First();

            document.Id.Should().Be("AnyId");
            document.Title.Should().Be("NewTitle");
            document.Description.Should().Be("AnyDescription");
        }

        [Test]
        public void remove_a_document()
        {
            const string id = "AnyId";
            connection.Execute(@"INSERT Documents(Id, Title, Description, Content, Language) 
                                VALUES (@id, NULL, NULL, NULL, NULL);",
                                new { id = id});

            new SqlDocumentRepository().Delete(id);

            var document = connection
                .Query<Document>(@"SELECT *" +
                                    "FROM [Documents]" +
                                    "WHERE Id = @Id;", new { Id = id });

            document.Should().BeEmpty();
        }

        [Test]
        public void load_a_list_with_all_documents()
        {
            for (var i = 1; i < 5; i++)
            {
                connection.Execute(@"INSERT Documents(Id, Title, Description, Content, Language) 
                                VALUES (@id, NULL, NULL, NULL, NULL);", new { id = i });
            }

            var documents = new SqlDocumentRepository().GetAllDocuments();

            documents.Should().Contain(doc => doc.Id == "1");
            documents.Should().Contain(doc => doc.Id == "2");
            documents.Should().Contain(doc => doc.Id == "3");
            documents.Should().Contain(doc => doc.Id == "4");
            documents.ToArray().Length.Should().Be(4);
        }
    }
}