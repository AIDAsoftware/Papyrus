using System;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class VersionRangeShould
    {
        private ProductVersion2 secondVersion;
        private ProductVersion2 fourthVersion;
        private Document2 spanishDocument;
        private Document2 englishDocument;

        [SetUp]
        public void SetUp()
        {
            secondVersion = new ProductVersion2("AnyProductVersionId", "AnyVersionName", DateTime.Now.AddDays(-2.0d));
            fourthVersion = new ProductVersion2("AnotherProductVersionId", "AnyVersionName", DateTime.Now);
            spanishDocument = new Document2(
                title: "Título",
                description: "Descripción",
                content: "Contenido"
            );
            englishDocument = new Document2(
                title: "Title",
                description: "Description",
                content: "Content"
            );
        }

        [Test]
        public void get_corresponding_document_for_a_given_language()
        {
            var versionRange = new VersionRange(
                fromVersion: secondVersion,
                toVersion: fourthVersion
            );
            versionRange.AddDocument("es-ES", spanishDocument);
            versionRange.AddDocument("en-GB", englishDocument);

            var document = versionRange.GetDocumentIn("es-ES");

            document.ShouldBeEquivalentTo(spanishDocument);
        }
    }
}