namespace Papyrus.WebServices
{
    using System.Web.Http;
    using LightInject;

    public static class WebApiConfig
    {
        public static ServiceContainer Container = new ServiceContainer();

        public static void Register(HttpConfiguration config)
        {
            Container.RegisterApiControllers();
            Container.EnableWebApi(config);

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
