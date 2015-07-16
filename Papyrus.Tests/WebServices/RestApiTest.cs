namespace Papyrus.Tests.WebServices
{
    using System;
    using System.Net.Http;
    using FluentAssertions;
    using NUnit.Framework;
    using Papyrus.WebServices.Controllers;

    [TestFixture]
    public class RestApiRunner : OwinRunner {
        [Test]
        public async void hello_world() {

            var client = new HttpClient();
            var response = client.GetAsync(baseAddress + "documents").Result;
            var document = await response.Content.ReadAsAsync<DocumentDto>();

            document.Title.Should().Be("Any");
            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        } 
    }
}