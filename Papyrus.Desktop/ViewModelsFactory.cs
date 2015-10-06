using System.Configuration;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Features.Documents;
using Papyrus.Desktop.Features.MainMenu;
using Papyrus.Desktop.Features.Topics;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Desktop {
    public static class ViewModelsFactory {
        public static MainMenuVM MainMenu() {
            return new MainMenuVM(ServicesFactory.Product());
        }

        public static DocumentsGridVM DocumentsGrid() {
            return new DocumentsGridVM(ServicesFactory.Document(), RepositoriesFactory.Product());
        }

        public static NewDocumentVM NewDocumentWindowVm()
        {
            return new NewDocumentVM(RepositoriesFactory.Product(), ServicesFactory.Document());
        }

        public static NewDocumentVM UpdateDocumentWindowVm(DocumentDetails document)
        {
            return new NewDocumentVM(RepositoriesFactory.Product(), ServicesFactory.Document(), document);
        }

        public static TopicsGridVM TopicsGrid()
        {
            return new TopicsGridVM(RepositoriesFactory.Topic());
        }
    }

    public static class ServicesFactory {
        public static ProductService Product() {
            return new ProductService(RepositoriesFactory.Product());
        }

        public static DocumentService Document() {
            return new DocumentService(RepositoriesFactory.Document());
        }
    }

    public static class RepositoriesFactory {
        private static DatabaseConnection CreateConnection() {
            var connectionString = ConfigurationManager.ConnectionStrings["Papyrus"].ToString();
            return new DatabaseConnection(connectionString);
        }

        public static ProductRepository Product() {
            return new SqlProductRepository(CreateConnection());
        }

        public static DocumentRepository Document() {
            return new SqlDocumentRepository(CreateConnection());
        }

        public static TopicRepository Topic()
        {
            return new SqlTopicRepository(CreateConnection());
        }
    }

}