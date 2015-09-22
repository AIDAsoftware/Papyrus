namespace Papyrus.Tests.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using Papyrus.Business.Documents;
    using Papyrus.Business.Documents.Exceptions;

    [TestFixture]
    public class DocumentServiceShould
    {
        private const string AnyId = "AnyId";
        private DocumentRepository repository;
        private DocumentService service;

        [SetUp]
        public void SetUp()
        {
            repository = Substitute.For<DocumentRepository>();
            repository.GetDocument(AnyId).Returns(            //TODO extract field
                Task.FromResult(new Document().WithId(AnyId))
            );
            service = new DocumentService(repository);
        }

        [Test]
        [ExpectedException(typeof(DocumentMustBeAssignedToAProductVersionException))]
        public async void fail_saving_document_without_productVersionId() {
            var document = new Document()
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario")
                .ForLanguage("es-Es");

            await service.Create(document);
        }


        [Test]
        [ExpectedException(typeof(DocumentMustHaveALanguageException))]
        public async void fail_saving_document_without_language() {
            var document = new Document()
                .ForProductVersion("anyProductVersion")
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario");

            await service.Create(document);
        }

        [Test]
        public async Task save_a_document_when_it_is_created()
        {
            var document = new Document()
                .ForProduct("anyProduct")
                .ForProductVersion("anyProductVersion")
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario")
                .ForLanguage("es-Es");

            await service.Create(document);

            repository.Received().Save(document);
            repository.Received()
                .Save(Arg.Is<Document>(x => !string.IsNullOrWhiteSpace(x.DocumentIdentity.Id)));
        }

        [Test]
        [ExpectedException(typeof(DocumentIdMustNotBeDefinedException))]
        public async Task throw_an_exception_when_try_to_create_a_document_with_an_id()
        {
            var document = new Document()
                .WithId(AnyId)
                .ForProductVersion("anyProductVersion")
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario")
                .ForLanguage("es-Es");
            await service.Create(document);
        }


        [Test]
        public async void get_a_saved_document_when_it_is_requested()
        {
            var id = "1";
            repository.GetDocument(id).Returns(Task.FromResult(new Document()
                .WithId(id))
            );

            var document = await service.GetDocumentById(id);

            document.DocumentIdentity.Id.Should().Be(id);
        }


        [Test]
        public async void update_a_given_document_when_it_is_modified()
        {
            var document = new Document().WithId(AnyId);

            document.WithTitle("Login en el sistema");
            await service.Update(document);

            repository.Received().Update(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentIdMustBeDefinedException))]
        public async Task throw_an_exception_when_try_to_update_a_document_without_id()
        {
            var document = new Document();
            await service.Update(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentNotFoundException))]
        public async Task throw_an_exception_when_try_to_update_a_non_existent_document()
        {
            var document = new Document().WithId("NoExistingId");
            await service.Update(document);
        }


        [Test]
        public async Task remove_a_given_document_when_it_is_deleted()
        {
            const string documentId = AnyId;
            await service.Remove(documentId);
            repository.Received().Delete(documentId);
        }

        [Test]
        [ExpectedException(typeof(DocumentNotFoundException))]
        public async Task throw_an_exception_when_try_to_delete_a_no_existing_document()
        {
            await service.Remove("NoExistingId");
        }


        [Test]
        public async Task return_a_list_of_documents_when_user_want_to_see_all_documents()
        {
            var anyId = AnyId;
            repository.GetAllDocuments().Returns(Task.FromResult(new List<Document> {
                new Document().WithId(anyId),
            }));

            var documents = await service.AllDocuments();

            documents.Should().Contain(x => x.DocumentIdentity.Id == anyId);
            documents.Length.Should().Be(1);
        }

    }
}
 