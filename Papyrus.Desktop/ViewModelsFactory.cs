using System.Configuration;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Features.Topics;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Desktop {
    public static class ViewModelsFactory {
        public static TopicsGridVM TopicsGrid()
        {
            return new TopicsGridVM(RepositoriesFactory.Topic());
        }

        public static TopicVM Topic(EditableTopic topic)
        {
            return new TopicVM(ServicesFactory.Topic(), topic);
        }

        public static MainWindowVM MainWindow()
        {
            return new MainWindowVM(RepositoriesFactory.Product());
        }
    }

    public static class ServicesFactory {
        public static ProductService Product() {
            return new ProductService(RepositoriesFactory.Product());
        }

        public static TopicService Topic()
        {
            return new TopicService(RepositoriesFactory.Topic());
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

        public static TopicRepository Topic()
        {
            return new SqlTopicRepository(CreateConnection());
        }
    }

}