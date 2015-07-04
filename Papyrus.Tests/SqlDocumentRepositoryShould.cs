namespace Papyrus.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Business;
    using Business.Documents;
    using Business.Documents.Exceptions;
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
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionForTests"].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();
        }


        [TearDown]
        public void TearDown()
        {
            connection.Execute(@"DELETE FROM Documents");
            connection.Dispose();
        }

        [Test]
        public async Task save_a_document()
        {
            var document = new Document()
                .WithId("AnyId");

            await new SqlDocumentRepository().Save(document);

            var requestedDocuments = LoadDocumentWithId("AnyId");
            requestedDocuments.ShouldBeEquivalentTo(document);
        }

        [Test]
        public async void load_a_document()
        {
            InsertDocumentWith(
                id: "AnyID", title: "AnyTitle", content: "AnyContent", description: "AnyDescription", language: "es"
            );

            var document = await new SqlDocumentRepository().GetDocument("AnyId");

            document.Id.Should().Be("AnyID");
            document.Title.Should().Be("AnyTitle");
            document.Description.Should().Be("AnyDescription");
            document.Content.Should().Be("AnyContent");
            document.Language.Should().Be("es");
        }

        [Test]
        public async Task update_a_document()
        {
            connection.Execute(@"INSERT Documents(Id, Title) 
                                VALUES (@id, @title);",
                                new { id = "AnyId", title = "AnyTitle" });

            var document = new Document()
                .WithId("AnyId")
                .WithTitle("NewTitle")
                .WithDescription("AnyDescription");

            await new SqlDocumentRepository().Update(document);
            var updatedDocument = LoadDocumentWithId("AnyId");

            updatedDocument.ShouldBeEquivalentTo(document);
        }

        [Test]
        public async void remove_a_document()
        {
            const string id = "AnyId";
            InsertDocumentWith(id: id);
            await new SqlDocumentRepository().Delete(id);

            var document = LoadDocumentWithId(id);

            document.Should().BeNull();
        }

        [Test]
        public async Task load_a_list_with_all_documents()
        {
            for (var i = 1; i < 5; i++) InsertDocumentWith(id: i.ToString());

            var documents = await new SqlDocumentRepository().GetAllDocuments();

            documents.Should().Contain(doc => doc.Id == "1");
            documents.Should().Contain(doc => doc.Id == "2");
            documents.Should().Contain(doc => doc.Id == "3");
            documents.Should().Contain(doc => doc.Id == "4");
            documents.ToArray().Length.Should().Be(4);
        }

        [Test]
        public void throw_an_exception_when_try_to_update_a_non_existent_document()
        {
            var document = new Document().WithId("NonExistentId");
            var updateTask = new Func<Task>(async () => await new SqlDocumentRepository().Update(document));
            updateTask.ShouldThrow<DocumentNotFoundException>();
        }

        private void InsertDocumentWith(string id, string title = null, string description = null, string content = null, string language = null)
        {
            connection.Execute(@"INSERT Documents(Id, Title, Description, Content, Language) 
                                VALUES (@Id, @Title, @Description, @Content, @Language);",
                                new {
                                    Id = id,
                                    Title = title,
                                    Description = description,
                                    Content = content,
                                    Language = language                    
                                });
        }

        private Document LoadDocumentWithId(string id)
        {
            var result = connection
                .Query<Document>(@"SELECT *" +
                                 "FROM [Documents]" +
                                 "WHERE Id = @Id;", new { Id = id }).ToArray();
            return result.Any() ? result.First() : null;
        }
    }
}