using System;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Documents;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class DocumentShould {
        [Test]
        public void not_be_created_with_an_empty_title() {
            Action documentCreation = () => new Document("", "Any Description", "Any Content", "Any Language");
            documentCreation.ShouldThrow<CannotCreateDocumentsWithoutTitleException>();
        }
        
        [Test]
        public void not_be_created_with_a_null_title() {
            Action documentCreation = () => new Document(null, "Any Description", "Any Content", "Any Language");
            documentCreation.ShouldThrow<CannotCreateDocumentsWithoutTitleException>();
        }
    }
}