namespace Papyrus.Tests.WebServices
{
    using FluentAssertions;
    using NUnit.Framework;
    using Papyrus.WebServices.Controllers;

    [TestFixture]
    public class RestApiRunner : OwinRunner {
        [Test]
        public async void hello_world() {
            var client = new RestClient(baseAddress);
            var document = await client.Get<DocumentDto>("documents");
            document.Title.Should().Be("Any");
        } 
    }
}