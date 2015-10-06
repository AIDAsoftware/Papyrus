using System.IO;
using System.Linq;
using System.Security.AccessControl;
using Papyrus.Business;

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
            repository.GetDocument(AnyId).Returns(
                Task.FromResult(new Document().WithTopicId(AnyId))
            );
            service = new DocumentService(repository);
        }

        [Test]
        [ExpectedException(typeof(DocumentMustBeAssignedToAProductVersionException))]
        public async void fail_saving_document_without_productVersionId() {
            var document = new Document()
                .ForProduct("AnyProductId")
                .ForLanguage("es-Es")
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario");

            await service.Create(document);
        }


        [Test]
        [ExpectedException(typeof(DocumentMustHaveALanguageException))]
        public async void fail_saving_document_without_language() {
            var document = new Document()
                .ForProduct("AnyProductId")
                .ForProductVersion("AnyProductVersionId")
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario");

            await service.Create(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentMustBeAssignedToAProductException))]
        public async void fail_saving_document_without_product() {
            var document = new Document()
                .ForProductVersion("AnyProductVersionId")
                .ForLanguage("es-ES")
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario");

            await service.Create(document);
        }

        [Test]
        public async Task save_a_document_when_it_is_created()
        {
            var document = new Document()
                .ForProduct("AnyProductId")
                .ForProductVersion("AnyProductVersionId")
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario")
                .ForLanguage("es-Es");

            await service.Create(document);

            repository.Received().Save(document);
            repository.Received()
                .Save(Arg.Is<Document>(x => !string.IsNullOrWhiteSpace(x.DocumentIdentity.TopicId)));
        }

        [Test]
        [ExpectedException(typeof(DocumentIdMustNotBeDefinedException))]
        public async Task throw_an_exception_when_try_to_create_a_document_with_an_id()
        {
            var document = new Document()
                .WithTopicId(AnyId)
                .ForProduct("AnyProductId")
                .ForProductVersion("anyProductVersionId")
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario")
                .ForLanguage("es-Es");
            await service.Create(document);
        }


        [Test]
        public async void get_a_saved_document_when_it_is_requested_by_document_id()
        {
            var id = "1";
            repository.GetDocument(id).Returns(Task.FromResult(new Document()
                .WithTopicId(id))
            );

            var document = await service.GetDocumentByTopicId(id);

            document.DocumentIdentity.TopicId.Should().Be(id);
        }


        [Test]
        public async void update_a_given_document_when_it_is_modified()
        {
            var document = new Document()
                .WithTopicId(AnyId)
                .ForProduct("AnyProductId")
                .ForProductVersion("AnyProductVersionId")
                .ForLanguage("es-ES");

            document.WithTitle("Login en el sistema");
            await service.Update(document);

            repository.Received().Update(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentIdMustBeDefinedException))]
        public async Task throw_an_exception_when_try_to_update_a_document_without_id()
        {
            var document = new Document()
                .ForProduct("AnyProductId")
                .ForProductVersion("AnyProductVersionId")
                .ForLanguage("es-ES");
            await service.Update(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentMustBeAssignedToAProductException))]
        public async Task throw_an_exception_when_try_to_update_a_document_which_is_not_assigned_to_a_product()
        {
            var document = new Document()
                .WithTopicId(AnyId)
                .ForProductVersion("AnyProductVersionId")
                .ForLanguage("es-ES");
            await service.Update(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentMustBeAssignedToAProductVersionException))]
        public async Task throw_an_exception_when_try_to_update_a_document_which_is_not_assigned_to_a_product_version()
        {
            var document = new Document()
                .WithTopicId(AnyId)
                .ForProduct("AnyProductId")
                .ForLanguage("es-ES");
            await service.Update(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentMustHaveALanguageException))]
        public async Task throw_an_exception_when_try_to_update_a_document_which_has_not_language()
        {
            var document = new Document()
                .WithTopicId(AnyId)
                .ForProduct("AnyProductId")
                .ForProductVersion("AnyProductVersionId");
            await service.Update(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentNotFoundException))]
        public async Task throw_an_exception_when_try_to_update_a_no_existing_document()
        {
            var document = new Document()
                .WithTopicId("NoExistingId")
                .ForProduct("AnyProductId")
                .ForProductVersion("AnyProductVersion")
                .ForLanguage("es-ES");
            await service.Update(document);
        }

        [Test]
        public async Task return_a_list_of_documents_when_user_want_to_see_all_documents()
        {
            var anyId = AnyId;
            repository.GetAllDocuments().Returns(Task.FromResult(new List<Document> {
                new Document().WithTopicId(anyId),
            }));

            var documents = await service.AllDocuments();

            documents.Should().Contain(x => x.DocumentIdentity.TopicId == anyId);
            documents.Length.Should().Be(1);
        }

        [Ignore]
        [Test]
        public async Task export_documents_to_a_folder() {
            var targetDirectory = new DirectoryInfo(@"c:\exportDirectory\docs");
            repository.GetAllDocuments().Returns(Task.FromResult(CreateSampleDocumentsList()));

            await service.ExportDocumentsToFolder(targetDirectory);

            var file = targetDirectory.GetFiles().FirstOrDefault();
            file.Name.Should().Be("Login en el sistema.md");
            File.ReadAllText(file.FullName).Should().Be("El usuario podrá acceder al sistema indicando su usuario");
        }

        private List<Document> CreateSampleDocumentsList() {
            var anyDocument = new Document()
                .WithTopicId(AnyId)
                .ForProduct("AnyProductId")
                .ForProductVersion("anyProductVersionId")
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario")
                .ForLanguage("es-Es");
            //var anyOtherDocument = new Document()
            //     .WithTopicId(AnyId + "b")
            //     .ForProduct("AnyProductId")
            //     .ForProductVersion("anyProductVersionId")
            //     .WithTitle("Creación de usuario")
            //     .WithDescription("Descripción de cómo registrarse en el sistema")
            //     .WithContent("El usuario podrá acceder crear una cuenta")
            //     .ForLanguage("es-Es");
  
            return new List<Document> { anyDocument };
        }
    }
}
 