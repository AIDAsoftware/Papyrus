namespace Papyrus.Tests.WebServices
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using FluentAssertions;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using NUnit.Framework;
    using Papyrus.Business.Documents;
    using Papyrus.Business.Documents.Exceptions;
    using Papyrus.WebServices;
    using Papyrus.WebServices.Models;

    [TestFixture]
    public class DocumentApiShould : OwinRunner
    {
        // TODO:
        //  when remove a document, it should return a 204 http status code
        //  when try to remove a no existing document, it should return a 404 http status code 

        private const string AnyId = "AnyId";
        private const string AnyTitle = "AnyTitle";
        private const string AnyContent = "AnyContent";
        private const string AnyDescription = "AnyDescription";
        private const string AnyLanguage = "AnyLanguage";
        private RestClient restClient;
        private DocumentService documentService;

        [SetUp]
        public void InitializeRestClientAndService()
        {
            restClient = new RestClient(baseAddress);
            documentService = SubstituteForDocumentService();
        }

        [Test]
        public async void when_looking_for_an_existing_document_it_should_return_a_dto_with_its_information()
        {
            GivenAWebApiWithADocumentServiceWhichWhenTryingToGetADocumentReturns(AnyDocument());
            
            var documentDto = await restClient.Get<DocumentDto>("documents/" + AnyId);

            ShouldBeADocumentDtoWithNoNullFields(documentDto);
        }

        [Test]
        public void return_a_404_http_status_code_when_looking_for_a_no_existing_document()
        {
            GivenAWebApiWithADocumentServiceWhichWhenTryingToGetADocumentReturns(null);

            Func<Task> asyncCall = async () => await restClient.Get<DocumentDto>("documents/" + AnyId);

            asyncCall.ShouldThrow<HttpResponseException>()
                .Which.Response.StatusCode.Should()
                .Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async void return_a_list_whit_a_documentdto_for_each_existing_document_when_getting_all_documents()
        {
            var expectedDocumentsList = new[] { new Document().WithTitle(AnyTitle) };
            GivenAWebApiWithADocumentServiceWhichWhenGettingAllDocumentsReturns(expectedDocumentsList);

            var documentDtos = await restClient.Get<DocumentDto[]>("documents/");

            documentDtos.Length.Should().Be(1);
            documentDtos[0].ShouldBeEquivalentTo(expectedDocumentsList[0]);
        }

        [Test]
        public async void return_a_201_http_status_code_when_creating_a_document()
        {
            WebApiConfig.Container.RegisterInstance(documentService);

            var document = new ComparableDocument().WithTitle(AnyTitle);
            var response = await restClient.PostAsJson("documents/", document);

            documentService.Received().Create(document);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async void return_a_200_http_status_code_when_updated_a_document()
        {
            WebApiConfig.Container.RegisterInstance(documentService);

            var document = new ComparableDocument().WithId(AnyId);
            var response = await restClient.PutAsJson("documents/" + document.Id, document);

            documentService.Received().Update(document);
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }

        [Test]
        public async Task return_a_404_http_status_code_when_updating_no_existing_document()
        {
            GivenAWebApiWithDocumentServiceWithoutDocuments();

            var document = new ComparableDocument().WithId(AnyId);
            var response = await restClient.PutAsJson("documents/" + document.Id, document);

            documentService.Received().Update(document);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task return_a_204_http_status_code_when_removed_a_document()
        {
            WebApiConfig.Container.RegisterInstance(documentService);

            var response = await restClient.Delete("documents/" + AnyId);

            documentService.Received().Remove(AnyId);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public async Task return_a_404_http_status_code_when_removing_a_no_existing_document()
        {
            documentService.Remove(Arg.Any<string>()).Throws<DocumentNotFoundException>();
            WebApiConfig.Container.RegisterInstance(documentService);

            var response = await restClient.Delete("documents/" + AnyId);

            documentService.Received().Remove(AnyId);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private void GivenAWebApiWithDocumentServiceWithoutDocuments()
        {
            documentService.
                Update(Arg.Any<Document>()).
                Throws<DocumentIdMustBeDefinedException>(); //TODO: change exception (confusion)
            WebApiConfig.Container.RegisterInstance(documentService);
        }

        private void GivenAWebApiWithADocumentServiceWhichWhenTryingToGetADocumentReturns(Document document)
        {
            documentService.GetDocumentById(AnyId).Returns(
                Task.FromResult(document)
                );
            WebApiConfig.Container.RegisterInstance(documentService);
        }

        private static void GivenAWebApiWithADocumentServiceWhichWhenGettingAllDocumentsReturns(Document[] expectedDocumentsList)
        {
            var documentService = SubstituteForDocumentService();
            documentService.AllDocuments().Returns(
                Task.FromResult(expectedDocumentsList)
                );
            WebApiConfig.Container.RegisterInstance(documentService);
        }

        private static DocumentService SubstituteForDocumentService()
        {
            var anyRepository = Substitute.For<DocumentRepository>();
            return Substitute.For<DocumentService>(anyRepository);
        }

        private static Document AnyDocument()
        {
            return new ComparableDocument()
                .WithId(AnyId)
                .WithTitle(AnyTitle)
                .WithContent(AnyContent)
                .WithDescription(AnyDescription)
                .ForLanguage(AnyLanguage);
        }

        private static void ShouldBeADocumentDtoWithNoNullFields(DocumentDto documentDto)
        {
            documentDto.Id.Should().Be(AnyId);
            documentDto.Title.Should().Be(AnyTitle);
            documentDto.Description.Should().Be(AnyDescription);
            documentDto.Content.Should().Be(AnyContent);
            documentDto.Language.Should().Be(AnyLanguage);
        }
    }

    internal class ComparableDocument : Document
    {
        public override bool Equals(object obj)
        {
            var other = obj as Document;
            if (other == null) return false;

            return (Title == other.Title && Description == other.Description &&
                    Language == other.Language && Content == other.Content);
        }
    }
}