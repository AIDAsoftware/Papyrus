using System.Configuration;
using Papyrus.Business.Products;
using Papyrus.Desktop.MainMenu;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Desktop {
    public static class ViewModelsFactory {
        public static MainMenuVM MainMenu() {
            return new MainMenuVM(ServicesFactory.Product());
        }
    }

    public static class ServicesFactory {
        public static ProductService Product() {
            return new ProductService(RepositoriesFactory.Product());
        }
    }

    public static class RepositoriesFactory {
        public static ProductRepository Product() {
            return new SqlProductRepository(CreateConnection());
        }

        private static DatabaseConnection CreateConnection() {
            var connectionString = ConfigurationManager.ConnectionStrings["Papyrus"].ToString();
            return new DatabaseConnection(connectionString);
        } 
    }

}