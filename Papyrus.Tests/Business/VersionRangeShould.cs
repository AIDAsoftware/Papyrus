using System;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class VersionRangeShould
    {
        private string secondVersion;
        private string fourthVersion;
        private Document2 spanishDocument;
        private Document2 englishDocument;

        [SetUp]
        public void InitializeDocuments()
        {
            spanishDocument = new Document2(
                title: "Título",
                description: "Descripción",
                content: "Contenido",
                language: "es-ES"
            );
            englishDocument = new Document2(
                title: "Title",
                description: "Description",
                content: "Content",
                language: "en-GB"
            );
        }

        [Test]
        public void get_corresponding_document_for_a_given_language()
        {
            var versionRange = new VersionRange(
                fromVersionId: "SecondVersionId",
                toVersionId: "FourthVersionId"
            );
            versionRange.AddDocument(spanishDocument);
            versionRange.AddDocument(englishDocument);

            var document = versionRange.GetDocumentIn("es-ES");

            document.ShouldBeEquivalentTo(spanishDocument);
        }
    }
}