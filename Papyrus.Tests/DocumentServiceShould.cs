namespace Papyrus.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Business;
    using Business.Documents;
    using Business.Documents.Exceptions;
    using FluentAssertions;
    using NSubstitute;
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
        public void throw_an_exception_when_try_to_change_the_id_of_a_document()
        {
            var document = new Document().WithId("AnyId");
            Action action = () => document.WithId("AnotherId");
            action.ShouldThrow<CannotModifyDocumentIdException>();
        }

        [Test]
        public async void get_a_saved_document_when_it_is_requested()
        {
            var id = "1";
            repository.GetDocument(id).Returns(Task.FromResult(new Document()
                .WithId(id))
            );

            var document = await service.GetDocumentById(id);

            document.Id.Should().Be(id);
        }


        [Test]
        public async void update_a_given_document_when_it_is_modified()
        {
            var document = new Document().WithId("AnyId");

            document.WithTitle("Login en el sistema");
            await service.Update(document);

            //TODO: Revisar, me pide el await, pero creo que no es necesario
            repository.Received().Update(document);
        }


        [Test]
        public void throw_an_exception_when_try_to_update_a_document_without_id()
        {
            var document = new Document();
            Func<Task> action = async () => await service.Update(document);
            action.ShouldThrow<DocumentIdCouldBeDefinedException>();
        }


        [Test]
        public async Task remove_a_given_document_when_it_is_deleted()
        {
            const string documentId = "AnyId";
            await service.Remove(documentId);
            repository.Received().Delete(documentId);
        }


        [Test]
        public async Task return_a_list_of_documents_when_user_want_to_see_all_documents()
        {
            repository.GetAllDocuments().Returns(Task.FromResult(new List<Document> {
                new Document().WithId("1"),
                new Document().WithId("2"),
                new Document().WithId("3")
            }));

            var documents = await service.AllDocuments();

            documents.Should().Contain(x => x.Id == "1");
            documents.Should().Contain(x => x.Id == "2");
            documents.Should().Contain(x => x.Id == "3");
            documents.Length.Should().Be(3);
        }

    }
}
 