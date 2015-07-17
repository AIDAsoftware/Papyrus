using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using LightInject;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using Owin;
using Papyrus.WebServices;
using Papyrus.WebServices.Controllers;

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
            WebApiConfig.Register(config);
            appBuilder.UseWebApi(config);
        }
    }

}
