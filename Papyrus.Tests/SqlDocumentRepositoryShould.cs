
namespace Papyrus.Tests
{
    using Papyrus.Infrastructure.Core.Database;
    using System.Linq;
    using System.Threading.Tasks;
    using Papyrus.Business.Documents;
    using Papyrus.Business.Documents.Exceptions;
    using FluentAssertions;
    using NUnit.Framework;

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
                .WithId("AnyId");

            await new SqlDocumentRepository(dbConnection).Save(document);

            var requestedDocuments = await LoadDocumentWithId("AnyId");
            requestedDocuments.ShouldBeEquivalentTo(document);
        }

        [Test]
        public async void load_a_document()
        {
            await InsertDocumentWith(
                id: "AnyID", title: "AnyTitle", content: "AnyContent", description: "AnyDescription", language: "es"
            );

            var document = await new SqlDocumentRepository(dbConnection).GetDocument("AnyId");

            document.Id.Should().Be("AnyID");
            document.Title.Should().Be("AnyTitle");
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
            await dbConnection.Execute(@"INSERT Documents(Id, Title) 
                                VALUES (@id, @title);",
                                new { id = "AnyId", title = "AnyTitle" });

            var document = new Document()
                .WithId("AnyId")
                .WithTitle("NewTitle")
                .WithDescription("AnyDescription");

            await new SqlDocumentRepository(dbConnection).Update(document);
            var updatedDocument = await LoadDocumentWithId("AnyId");

            updatedDocument.ShouldBeEquivalentTo(document);
        }

        [Test]
        public async void remove_a_document()
        {
            const string id = "AnyId";
            await InsertDocumentWith(id: id);
            await new SqlDocumentRepository(dbConnection).Delete(id);

            var document = await LoadDocumentWithId(id);

            document.Should().BeNull();
        }

        [Test]
        public async Task load_a_list_with_all_documents()
        {
            await InsertDocumentWith(id: "1");
            await InsertDocumentWith(id: "2");

            var documents = await new SqlDocumentRepository(dbConnection).GetAllDocuments();

            documents.Should().Contain(doc => doc.Id == "1");
            documents.Should().Contain(doc => doc.Id == "2");
            documents.ToArray().Length.Should().Be(2);
        }


        private async Task InsertDocumentWith(string id, string title = null, string description = null, string content = null, string language = null)
        {
            await dbConnection.Execute(@"INSERT Documents(Id, Title, Description, Content, Language) 
                                VALUES (@Id, @Title, @Description, @Content, @Language);",
                                new {
                                    Id = id,
                                    Title = title,
                                    Description = description,
                                    Content = content,
                                    Language = language                    
                                });
        }

        private async Task<Document> LoadDocumentWithId(string id)
        {
            var result = await dbConnection
                .Query<Document>(@"SELECT Id, Title, Content, Description, Language " +
                                 "FROM [Documents]" +
                                 "WHERE Id = @Id;", new { Id = id });
            return result.FirstOrDefault();
        }
    }
}