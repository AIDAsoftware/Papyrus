using System;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.Topics.Exceptions;
using Papyrus.Business.VersionRanges;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class VersionRangeShould
    {
        private string secondVersion;
        private string fourthVersion;
        private Document spanishDocument;
        private Document englishDocument;
        private readonly ProductVersion version2 = new ProductVersion("SecondVersionId", "2.0", DateTime.Today.AddDays(-2));
        private readonly ProductVersion version4 = new ProductVersion("FourthVersionId", "4.0", DateTime.Today);

        [SetUp]
        public void InitializeDocuments()
        {
            spanishDocument = new Document(
                title: "Título",
                description: "Descripción",
                content: "Contenido",
                language: "es-ES"
            );
            englishDocument = new Document(
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
                fromVersion: version2, 
                toVersion: version4
            );
            versionRange.AddDocument(spanishDocument);
            versionRange.AddDocument(englishDocument);

            var document = versionRange.GetDocumentIn("es-ES");

            document.ShouldBeEquivalentTo(spanishDocument);
        }

        [Test]
        public void not_be_created_if_its_first_version_is_bigger_than_its_second_one() {
            Action versionRangeCreation = () => new VersionRange(version4, version2);
            versionRangeCreation.ShouldThrow<VersionRangeCannotBeDescendentException>();
        }

    }
}