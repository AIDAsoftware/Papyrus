namespace Papyrus.Tests.WebServices.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using NUnit.Framework;
    using Papyrus.Business.Documents;
    using Papyrus.Business.Documents.Exceptions;
    using Papyrus.WebServices.Controllers;
    using Papyrus.WebServices.Models;

    [TestFixture]
    public class DocumentControllerShould
    {
        // TODO:
        //  when looking for a no existing Document, it should return an error message
        //  when getting all Documents, it should return a list of dtos corresponding to all existant documents
        //  when creating a document, it should return a message which confirms creation
        //  when update a document, it should return a message which confirms the update
        //  when try to update a no existing document, it should return a message which confirms the update
        //  when remove a document, it should return a message which confirms elimination
        //  when try to remove a no existing document, it should return a fail message 

        private const string AnyId = "AnyId";
        private const string AnyTitle = "AnyTitle";
        private const string AnyContent = "AnyContent";
        private const string AnyDescription = "AnyDescription";
        private const string AnyLanguage = "AnyLanguage";

        [Test]
        public async void when_looking_for_an_existing_document_it_should_return_a_dto_with_its_information()
        {
            var documentService = Substitute.For<DocumentService>(new NotImplementedRepository());
            documentService.GetDocumentById(AnyId).Returns(
                Task.FromResult(AnyDocument())
            );

            var documentDto = await new DocumentsController(documentService).Get("AnyId");

            ShouldBeADocumentDtoWithNoNullFields(documentDto);
        }

        [Test]
        [ExpectedException(typeof(DocumentNotFoundException))]
        public async void when_looking_for_a_no_existing_document()
        {
            var documentService = Substitute.For<DocumentService>(new NotImplementedRepository());
            documentService.GetDocumentById(Arg.Any<string>()).Returns(Task.FromResult((Document) null));

            await new DocumentsController(documentService).Get(AnyId);
        }


        private static void ShouldBeADocumentDtoWithNoNullFields(DocumentDto documentDto)
        {
            documentDto.Title.Should().Be(AnyTitle);
            documentDto.Description.Should().Be(AnyDescription);
            documentDto.Content.Should().Be(AnyContent);
            documentDto.Language.Should().Be(AnyLanguage);
        }

        private static Document AnyDocument()
        {
            return new Document()
                .WithId(AnyId)
                .WithTitle(AnyTitle)
                .WithContent(AnyContent)
                .WithDescription(AnyDescription)
                .ForLanguage(AnyLanguage);
        }
    }

    public class NotImplementedRepository : DocumentRepository
    {
        public Task Save(Document document)
        {
            throw new System.NotImplementedException();
        }

        public Task<Document> GetDocument(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(Document document)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string documentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Document>> GetAllDocuments()
        {
            throw new System.NotImplementedException();
        }
    }
}