namespace Papyrus.Tests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DocumentServiceShould
    {

        [Test]
        public void save_a_document_when_it_is_created()
        {
            var document = new Document()
                .WithTitle("Login en el sistema")
                .WithDescription("Modos de acceso disponibles a SIMA 2")
                .WithContent("El usuario podrá acceder al sistema indicando su usuario")
                .ForLanguage("es-Es");

            var repository = Substitute.For<DocumentRepository>();
            var service = new DocumentService(repository);
            
            service.Create(document);

            repository.Received().Save(document);
        }


        // Dado un documento guardado con identificador "1"
        // Cuando el usuario solicita un documento con identificador "1"
        // El sistema devolverá el documento con identificador "1"
        [Test]
        public void get_a_saved_document_when_it_is_requested()
        {
            var id = "1";

            var repository = Substitute.For<DocumentRepository>();
            repository.GetDocument(id).Returns(new Document()
                    .WithId(id)
                );

            var service = new DocumentService(repository);
            var document = service.GetDocumentById(id);

            repository.Received().GetDocument(id);
            document.Id.Should().Be(id);
        }


    }
}