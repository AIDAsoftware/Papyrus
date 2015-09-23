namespace Papyrus.Tests.Business
{
    using NUnit.Framework;
    using Papyrus.Business.Documents;
    using Papyrus.Business.Documents.Exceptions;

    public class DocumentShould
    {
        [Test]
        [ExpectedException(typeof(CannotModifyDocumentIdException))]
        public void throw_an_exception_when_try_to_change_its_id()
        {
            var document = new Document().WithTopicId("AnyId");
            document.WithTopicId("AnotherId");
        }
    }
}