using System;
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
                .WithId("AnyId")
                .ForProduct("AnyProduct")
                .ForProductVersion("AnyProductVersion")
                .ForLanguage("AnyLanguage");

            await new SqlDocumentRepository(dbConnection).Save(document);

            var requestedDocuments = await LoadDocumentWith("AnyId", "AnyProduct", "AnyProductVersion", "AnyLanguage");
            requestedDocuments.ShouldBeEquivalentTo(document);
        }

        [Test]
        public async void load_a_document()
        {
            await InsertDocumentWith(
                id: "AnyID", productId: "AnyProduct", productVersionId: "AnyProductVersion", 
                language: "es", title: "AnyTitle", description: "AnyDescription", content: "AnyContent");

            var document = await new SqlDocumentRepository(dbConnection).GetDocument("AnyID");

            document.DocumentIdentity.Id.Should().Be("AnyID");
            document.DocumentIdentity.ProductId.Should().Be("AnyProduct");
            document.DocumentIdentity.VersionId.Should().Be("AnyProductVersion");
            document.DocumentIdentity.Language.Should().Be("es");
            document.Title.Should().Be("AnyTitle");
            document.Description.Should().Be("AnyDescription");
            document.Content.Should().Be("AnyContent");
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
            await InsertDocumentWith(id: "AnyId", productId: "AnyProductId", productVersionId: "AnyProductVersionId",
                language: "es-ES", title: "AnyTitle", description: "AnyDescription", content: "AnyContent");

            var document = new Document()
                .WithId("AnyId")
                .ForProduct("AnyProductId")
                .ForLanguage("es-ES")
                .ForProductVersion("AnyProductVersionId")
                .WithTitle("NewTitle")
                .WithDescription("NewDescription")
                .WithContent("NewContent");

            await new SqlDocumentRepository(dbConnection).Update(document);
            var updatedDocument = await LoadDocumentWith("AnyId", "AnyProductId", "AnyProductVersionId", "es-ES");

            updatedDocument.ShouldBeEquivalentTo(document);
        }

        [Test]
        public async void remove_a_document()
        {
            const string id = "AnyId";
            await InsertDocumentWith(id: id, productId: "AnyProductId", productVersionId: "AnyProductVersionId", language: "es-ES");
            await new SqlDocumentRepository(dbConnection).Delete(id);

            var document = await LoadDocumentWith(id, "AnyProductId", "AnyProductVersionId", "es-ES");

            document.Should().BeNull();
        }

        [Test]
        public async Task load_a_list_with_all_documents()
        {
            await InsertDocumentWith(id: "1", productId: "AnyProduct", productVersionId: "AnyProductVersionId", language: "es-ES");
            await InsertDocumentWith(id: "2", productId: "AnotherProduct", productVersionId: "AnyProductVersionId", language: "es-ES");

            var documents = await new SqlDocumentRepository(dbConnection).GetAllDocuments();

            documents.Should().Contain(doc => doc.DocumentIdentity.Id == "1");
            documents.Should().Contain(doc => doc.DocumentIdentity.Id == "2");
            documents.ToArray().Length.Should().Be(2);
        }


        private async Task InsertDocumentWith(string id, string productId, string productVersionId, string language, string title = null, string description = null, string content = null)
        {
            await dbConnection.Execute(@"INSERT Documents(Id, ProductId, ProductVersionId, Language, Title, Description, Content) 
                                VALUES (@Id, @ProductId, @ProductVersionId, @Language, @Title, @Description, @Content);",
                                new {
                                    Id = id,
                                    ProductId = productId,
                                    ProductVersionId = productVersionId,
                                    Language = language,                    
                                    Title = title,
                                    Description = description,
                                    Content = content
                                });
        }

        private async Task<Document> LoadDocumentWith(string id, string productId, string productVersionId, string language)
        {
            var result = (await dbConnection
                .Query<dynamic>(sql: @"SELECT Id, ProductId, ProductVersionId, Language, Title, Content, Description " +
                                 "FROM [Documents] " +
                                 "WHERE Id = @Id AND ProductId = @ProductId " +
                                                "AND ProductVersionId = @ProductVersionId " +
                                                "AND Language = @Language;",
                                 param: new {
                                    Id = id,
                                    ProductId = productId,
                                    ProductVersionId = productVersionId,
                                    Language = language
                                 })).FirstOrDefault();

            if (result == null) return null;

            return new Document()
                .WithId(result.Id)
                .WithTitle(result.Title)
                .WithContent(result.Content)
                .WithDescription(result.Description)
                .ForLanguage(result.Language)
                .ForProduct(result.ProductId)
                .ForProductVersion(result.ProductVersionId);

        }
    }
}