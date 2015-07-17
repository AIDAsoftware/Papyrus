using System.Configuration;
using Papyrus.Business.Documents;

namespace Papyrus.WebServices
{
    using System.Web.Http;
    using LightInject;
    using Infrastructure.Core.Database;

    public static class WebApiConfig
    {
        public static ServiceContainer Container = new ServiceContainer();

        public static void Register(HttpConfiguration config)
        {
            Container.RegisterApiControllers();
            Container.EnableWebApi(config);

            var connectionString = ConfigurationManager.ConnectionStrings["Papyrus"].ToString();
            var databaseConnection = new DatabaseConnection(connectionString);
            var documentService = new DocumentService(new SqlDocumentRepository(databaseConnection));
            Container.RegisterInstance(documentService);

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
