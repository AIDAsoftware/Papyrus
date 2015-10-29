using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters {
    public class MkDocsExporter {
        private readonly TopicQueryRepository repository;

        public MkDocsExporter(TopicQueryRepository repository) {
            this.repository = repository;
        }

        public async Task ExportDocumentsForProductToFolder(string productId, List<ProductVersion> versions, DirectoryInfo testDirectory)
        {
            var topics = await repository.GetEditableTopicsForProduct(productId);
            foreach (var topic in topics)
            {
                await ExportTopic(topic, testDirectory);
            }
        }

        private async Task ExportTopic(ExportableTopic topic, DirectoryInfo directory)
        {
            await topic.ExportTopicIn(directory);
        }
    }
}
