namespace Papyrus.Tests
{
    using System;
    using Business;
    using FluentAssertions;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using NUnit.Framework;

    [TestFixture]
    public class DocumentServiceShould
    {
        private DocumentRepository repository;
        private DocumentService service;

        [SetUp]
        public void SetUp()
        {
            repository = Substitute.For<DocumentRepository>();
            service = new DocumentService(repository);
        }


        [Test]
        public void save_a_document_when_it_is_created()
        {
            var document = new Document()
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario")
                .ForLanguage("es-Es");

            service.Create(document);

            repository.Received().Save(document);
            repository.Received().Save(Arg.Is<Document>(x => !string.IsNullOrWhiteSpace(x.Id)));
        }

        [Test]
        public void throw_an_exception_when_try_to_create_a_document_with_an_id()
        {
            var document = new Document().WithId("AnyId");

            Action action = () => service.Create(document);
            action.ShouldThrow<DocumentIdCouldNotBeDefinedException>();
        }

        [Test]
        public void get_a_saved_document_when_it_is_requested()
        {
            var id = "1";

            repository.GetDocument(id).Returns(new Document()
                    .WithId(id)
                );

            var document = service.GetDocumentById(id);

            repository.Received().GetDocument(id);
            document.Id.Should().Be(id);
        }


        [Test]
        public void update_a_given_document_when_it_is_modified()
        {
            var document = new Document().WithId("AnyId");

            document.WithTitle("Login en el sistema");
            service.Update(document);

            repository.Received().Update(document);
        }


        [Test]
        public void throw_an_exception_when_try_to_update_a_document_without_id()
        {
            var document = new Document();

            Action action = () => service.Update(document);
            action.ShouldThrow<DocumentIdCouldBeDefinedException>();
        }


        [Test]
        public void remove_a_given_document_when_it_is_deleted()
        {
            const string documentId = "AnyId";
            service.Remove(documentId);
            repository.Received().Delete(documentId);
        }

    }
}
 