namespace Papyrus.Tests
{
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
         
    }
}