namespace Papyrus.Tests.WebServices {
    using System;
    using System.Web.Http;
    using LightInject;
    using Microsoft.Owin.Hosting;
    using NUnit.Framework;
    using Owin;
    using Papyrus.WebServices;

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
            WebApiConfig.Container = new ServiceContainer();
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            appBuilder.UseWebApi(config);
        }
    }

}
