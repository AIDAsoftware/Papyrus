using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Documents;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.Infrastructure.Repositories
{
    [TestFixture]
    public class SqlDocumentRepositoryShould : SqlTest
    {
        [SetUp]
        public async void TruncateDataBase()
        {
            await dbConnection.Execute("TRUNCATE TABLE Documents");
        }

        [Test]
        public async Task save_a_document()
        {
            var document = new Document()
                .WithId("AnyId").ForProductVersion("AnyProductVersion").ForLanguage("AnyLanguage");

            await new SqlDocumentRepository(dbConnection).Save(document);

            var requestedDocuments = await LoadDocumentWith("AnyId", "AnyProductVersion", "AnyLanguage");
            requestedDocuments.ShouldBeEquivalentTo(document);
        }

        [Test]
        public async void load_a_document()
        {
            await InsertDocumentWith(
                id: "AnyID", productVersionId: "AnyProductVersion", title: "AnyTitle", content: "AnyContent", description: "AnyDescription", language: "es"
            );

            var document = await new SqlDocumentRepository(dbConnection).GetDocument("AnyId");

            document.Id.Should().Be("AnyID");
            document.Title.Should().Be("AnyTitle");
            document.ProductVersionId.Should().Be("AnyProductVersion");
            document.Description.Should().Be("AnyDescription");
            document.Content.Should().Be("AnyContent");
            document.Language.Should().Be("es");
        }

        [Test]
        public async void return_null_when_try_to_load_an_no_existing_document()
        {
            var document = await new SqlDocumentRepository(dbConnection).GetDocument("AnyId");

            document.Should().Be(null);
        }

        [Test]
        public async Task update_a_document()
        {
            await dbConnection.Execute(@"INSERT Documents(Id, ProductVersionId, Language, Title) 
                                VALUES (@id, @productVersionId, @language, @title);",
                                new {   id = "AnyId", 
                                        productVersionId = "AnyProductVersionId", 
                                        language = "es-ES", 
                                        title = "AnyTitle" });

            var document = new Document()
                .WithId("AnyId")
                .ForLanguage("es-ES")
                .ForProductVersion("AnyProductVersionId")
                .WithTitle("NewTitle")
                .WithDescription("AnyDescription");

            await new SqlDocumentRepository(dbConnection).Update(document);
            var updatedDocument = await LoadDocumentWith("AnyId", "AnyProductVersionId", "es-ES");

            updatedDocument.ShouldBeEquivalentTo(document);
        }

        [Test]
        public async void remove_a_document()
        {
            const string id = "AnyId";
            await InsertDocumentWith(id: id, productVersionId: "AnyProductVersionId", language: "es-ES");
            await new SqlDocumentRepository(dbConnection).Delete(id);

            var document = await LoadDocumentWith(id, "AnyProductVersionId", "es-ES");

            document.Should().BeNull();
        }

        [Test]
        public async Task load_a_list_with_all_documents()
        {
            await InsertDocumentWith(id: "1", productVersionId: "AnyProductVersionId", language: "es-ES");
            await InsertDocumentWith(id: "2", productVersionId: "AnyProductVersionId", language: "es-ES");

            var documents = await new SqlDocumentRepository(dbConnection).GetAllDocuments();

            documents.Should().Contain(doc => doc.Id == "1");
            documents.Should().Contain(doc => doc.Id == "2");
            documents.ToArray().Length.Should().Be(2);
        }


        private async Task InsertDocumentWith(string id, string productVersionId, string language, string title = null, string description = null, string content = null)
        {
            await dbConnection.Execute(@"INSERT Documents(Id, ProductVersionId, Language, Title, Description, Content) 
                                VALUES (@Id, @ProductVersionId, @Language, @Title, @Description, @Content);",
                                new {
                                    Id = id,
                                    ProductVersionId = productVersionId,
                                    Language = language,                    
                                    Title = title,
                                    Description = description,
                                    Content = content
                                });
        }

        private async Task<Document> LoadDocumentWith(string id, string productVersionId, string language)
        {
            var result = await dbConnection
                .Query<Document>(sql: @"SELECT Id, ProductVersionId, Language, Title, Content, Description " +
                                 "FROM [Documents] " +
                                 "WHERE Id = @Id AND ProductVersionId = @ProductVersionId AND Language = @language;",
                                 param: new {
                                    Id = id,
                                    ProductVersionId = productVersionId,
                                    Language = language
                                 });
            return result.FirstOrDefault();
        }
    }
}