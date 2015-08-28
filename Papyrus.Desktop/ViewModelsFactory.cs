using System.Configuration;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Desktop.Documents;
using Papyrus.Desktop.MainMenu;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Desktop {
    public static class ViewModelsFactory {
        public static MainMenuVM MainMenu() {
            return new MainMenuVM(ServicesFactory.Product());
        }

        public static DocumentsGridVM DocumentsGrid() {
            return new DocumentsGridVM(ServicesFactory.Document());
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
    }

}