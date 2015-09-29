using System;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class VersionRangeShould
    {
        // TODO:
        //   get the document for a given language
        //   know if it contains a given version
        // Examples:
        //   Given a Range with two Languages (es, en) when I request for "es" it should give me its spanish document.
        //   Given a Range which contains versions from 2 to 4 it should return:
        //      - false when I request by the 1
        //      - true when I request by the 2
        //      - true when I request by the 3
        //      - true when I request by the 4
        //      - false when I request by the 5

        [Test]
        public void get_corresponding_document_for_a_given_language()
        {
            var secondVersion = new ProductVersion2("AnyProductVersionId", "AnyVersionName", DateTime.Now.AddDays(-2.0d));
            var fourthVersion = new ProductVersion2("AnotherProductVersionId", "AnyVersionName", DateTime.Now);
            var versionRange = new VersionRange(
                fromVersion: secondVersion,
                toVersion: fourthVersion
            );
            var spanishDocument = new Document2(
                title: "Título", 
                description: "Descripción", 
                content:"Contenido"
                );
            versionRange.AddDocument("es-ES", spanishDocument);
            var englishDocument = new Document2(
                title: "Title",
                description: "Description",
                content: "Content"    
            );
            versionRange.AddDocument("en-GB", englishDocument);

            var document = versionRange.GetDocumentIn("es-ES");

            document.ShouldBeEquivalentTo(spanishDocument);
        }
    }
}