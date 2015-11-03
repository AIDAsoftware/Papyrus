using System.Configuration;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Features.Topics;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Desktop {
    public static class ViewModelsFactory {
        public static TopicsGridVM TopicsGrid()
        {
            return new TopicsGridVM(RepositoriesFactory.QueryTopic(), RepositoriesFactory.Product(), new MkDocsExporter(RepositoriesFactory.QueryTopic(), RepositoriesFactory.Product()));
        }

        public static TopicVM Topic(EditableTopic topic)
        {
            return new TopicVM(ServicesFactory.Topic(), RepositoriesFactory.Product(), topic);
        }

        public static MainWindowVM MainWindow()
        {
            return new MainWindowVM();
        }

        public static VersionRangeVM VersionRange(EditableVersionRange versionRange)
        {
            return new VersionRangeVM(versionRange);
        }
    }

    public static class ServicesFactory {
        public static ProductService Product() {
            return new ProductService(RepositoriesFactory.Product());
        }

        public static TopicService Topic()
        {
            return new TopicService(RepositoriesFactory.Topic(), new VersionRangeCollisionDetector(RepositoriesFactory.Product()));
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

        public static TopicCommandRepository Topic()
        {
            return new SqlTopicCommandRepository(CreateConnection());
        }

        public static TopicQueryRepository QueryTopic()
        {
            return new SqlTopicQueryRepository(CreateConnection());
        }
    }

}