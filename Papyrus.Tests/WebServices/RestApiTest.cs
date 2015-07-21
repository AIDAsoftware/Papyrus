using Papyrus.WebServices.Models;

namespace Papyrus.Tests.WebServices
{
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class RestApiRunner : OwinRunner {
        [Ignore]
        [Test]
        public async void hello_world() {
            var client = new RestClient(baseAddress);
            var document = await client.Get<DocumentDto>("documents");
            document.Title.Should().Be("Any");
        } 
    }
}