using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Util.Command;

namespace Papyrus.Desktop.Features.Topics {
    public class TopicsGridVM
    {
        private readonly TopicRepository topicRepository;

        public ObservableCollection<TopicSummary> TopicsToList { get; protected set; }
        public TopicSummary SelectedTopic { get; set; }
        public DisplayableProduct SelectedProduct { get; set; }

        public TopicsGridVM()
        {
            TopicsToList = new ObservableCollection<TopicSummary>();
            RefreshTopics = RelayAsyncSimpleCommand.Create(LoadAllTopics, CanLoadAllTopics);
            SelectedProduct = new DisplayableProduct {ProductName = "Organizer", ProductId = "OrganizerId" };
        }

        private bool canLoad;
        private bool CanLoadAllTopics()
        {
            return canLoad;
        }

        public TopicsGridVM(TopicRepository topicRepository) : this()
        {
            this.topicRepository = topicRepository;
        }

        public async Task Initialize()
        {
            await LoadAllTopics();
        }

        private async Task LoadAllTopics()
        {
            canLoad = false;
            TopicsToList.Clear();
            (await topicRepository.GetAllTopicsSummaries())
                .Where(t => t.Product.ProductId == SelectedProduct.ProductId)
                .ToList()
                .ForEach(topic => TopicsToList.Add(topic));
            canLoad = true;
        }

        public IAsyncCommand RefreshTopics { get; set; }

        public async void RefreshDocuments()
        {
            await LoadAllTopics();
        }

        public async Task<EditableTopic> GetEditableTopicById(string topicId)
        {
            return await topicRepository.GetEditableTopicById(topicId);
        }
    }

    public class DesignModeTopicsGridVM : TopicsGridVM
    {
        public DesignModeTopicsGridVM()
        {
            TopicsToList = new ObservableCollection<TopicSummary>
            {
                new TopicSummary
                {
                    LastDocumentTitle = "Login",
                    LastDocumentDescription = "Explicación",
                    VersionName = "2.0",
                    Product = new DisplayableProduct {ProductId = "ProductId", ProductName = "Opportunity"}
                },
                new TopicSummary
                {
                    LastDocumentTitle = "Llamadas",
                    LastDocumentDescription = "Explicación",
                    VersionName = "3.0",
                    Product = new DisplayableProduct {ProductId = "ProductId", ProductName = "Opportunity"}
                }
            };
        }
    }
}
