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
                .WithTopicId("AnyId")
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
                topicId: "AnyID", productId: "AnyProduct", productVersionId: "AnyProductVersion", 
                language: "es", title: "AnyTitle", description: "AnyDescription", content: "AnyContent");

            var document = await new SqlDocumentRepository(dbConnection).GetDocument("AnyID");

            document.DocumentIdentity.TopicId.Should().Be("AnyID");
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
            await InsertDocumentWith(topicId: "AnyId", productId: "AnyProductId", productVersionId: "AnyProductVersionId",
                language: "es-ES", title: "AnyTitle", description: "AnyDescription", content: "AnyContent");

            var document = new Document()
                .WithTopicId("AnyId")
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
        public async void remove_a_document()    //TODO: remove a document by Document Identity instead of by document topicId?
        {
            const string id = "AnyId";
            await InsertDocumentWith(topicId: id, productId: "AnyProductId", productVersionId: "AnyProductVersionId", language: "es-ES");
            await new SqlDocumentRepository(dbConnection).Delete(id);

            var document = await LoadDocumentWith(id, "AnyProductId", "AnyProductVersionId", "es-ES");

            document.Should().BeNull();
        }

        [Test]                       //TODO: Is this test complete?
        public async Task load_a_list_with_all_documents()
        {
            await InsertDocumentWith(topicId: "1", productId: "AnyProductId", productVersionId: "AnyProductVersionId", language: "es-ES");
            await InsertDocumentWith(topicId: "2", productId: "AnotherProductId", productVersionId: "AnyProductVersionId", language: "es-ES");

            var documents = await new SqlDocumentRepository(dbConnection).GetAllDocuments();

            documents.Should().Contain(doc => doc.DocumentIdentity.TopicId == "1");
            documents.Should().Contain(doc => doc.DocumentIdentity.TopicId == "2");
            documents.ToArray().Length.Should().Be(2);
        }


        private async Task InsertDocumentWith(string topicId, string productId, string productVersionId, string language, string title = null, string description = null, string content = null)
        {
            await dbConnection.Execute(@"INSERT Documents(TopicId, ProductId, ProductVersionId, Language, Title, Description, Content) 
                                VALUES (@TopicId, @ProductId, @ProductVersionId, @Language, @Title, @Description, @Content);",
                                new {
                                    TopicId = topicId,
                                    ProductId = productId,
                                    ProductVersionId = productVersionId,
                                    Language = language,                    
                                    Title = title,
                                    Description = description,
                                    Content = content
                                });
        }

        private async Task<Document> LoadDocumentWith(string topicId, string productId, string productVersionId, string language)
        {
            var result = (await dbConnection
                .Query<dynamic>(sql: @"SELECT TopicId, ProductId, ProductVersionId, Language, Title, Content, Description " +
                                 "FROM [Documents] " +
                                 "WHERE TopicId = @TopicId AND ProductId = @ProductId " +
                                                "AND ProductVersionId = @ProductVersionId " +
                                                "AND Language = @Language;",
                                 param: new {
                                    TopicId = topicId,
                                    ProductId = productId,
                                    ProductVersionId = productVersionId,
                                    Language = language
                                 })).FirstOrDefault();

            if (result == null) return null;

            return new Document()
                .WithTopicId(result.TopicId)
                .WithTitle(result.Title)
                .WithContent(result.Content)
                .WithDescription(result.Description)
                .ForLanguage(result.Language)
                .ForProduct(result.ProductId)
                .ForProductVersion(result.ProductVersionId);

        }
    }
}