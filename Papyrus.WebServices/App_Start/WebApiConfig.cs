using System.Configuration;
using Papyrus.Business.Documents;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.WebServices
{
    using System.Web.Http;
    using LightInject;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new ServiceContainer();
            container.RegisterApiControllers();
            container.EnableWebApi(GlobalConfiguration.Configuration);

            var connectionString = ConfigurationManager.ConnectionStrings["Papyrus"].ToString();
            var databaseConnection = new DatabaseConnection(connectionString);
            var documentService = new DocumentService(new SqlDocumentRepository(databaseConnection));
            container.RegisterInstance(documentService);

            // Web API configuration and services
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            
        }
    }

}
