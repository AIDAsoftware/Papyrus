namespace Papyrus.Tests.WebServices
{
    using System;
    using System.Net.Http;
    using FluentAssertions;
    using Microsoft.Owin.Hosting;
    using NUnit.Framework;
    using Papyrus.WebServices.Controllers;

    [TestFixture]
    public class RestApiTest
    {
        [Test]
        public async void hello_world()
        {
//            DocumentDto doc = new DocumentDto();
            string baseAddress = "http://localhost:8889/papyrusapi/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "documents").Result;
                var document = await response.Content.ReadAsAsync<DocumentDto>();

                document.Title.Should().Be("Any");
                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        } 
    }
}