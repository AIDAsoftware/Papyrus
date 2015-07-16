using System;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using Owin;
using Papyrus.WebServices;

namespace Papyrus.Tests.WebServices {
    [TestFixture]
    public abstract class OwinRunner {
        private IDisposable webServer;
        protected const string baseAddress = "http://localhost:8889/papyrusapi/";

        [SetUp]
        public void SetUp() {
            webServer = WebApp.Start<Startup>(url: baseAddress);
        }

        [TearDown]
        public void TearDown() {
            webServer.Dispose();
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            // Takes all the referenced DLLs in the project
            // Finds all the WebApi controllers and runs them all
            WebApiConfig.Register(config);

            appBuilder.UseWebApi(config);
        }
    }
}