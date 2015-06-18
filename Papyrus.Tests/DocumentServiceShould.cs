namespace Papyrus.Tests
{
    using Business;
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

    }
}