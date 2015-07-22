namespace Papyrus.WebServices
{
    using System.Configuration;
    using System.Web.Http;
    using Business.Documents;
    using Infrastructure.Core.Database;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start() {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var connectionString = ConfigurationManager.ConnectionStrings["Papyrus"].ToString();
            var databaseConnection = new DatabaseConnection(connectionString);
            var documentService = new DocumentService(new SqlDocumentRepository(databaseConnection));
            WebApiConfig.Container.RegisterInstance(documentService);
        }
    }
}
