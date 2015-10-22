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

        public ObservableCollection<TopicSummary> TopicsToList { get; }
        public TopicSummary SelectedTopic { get; set; }
        public DisplayableProduct SelectedProduct { get; set; }

        public TopicsGridVM()
        {
            TopicsToList = new ObservableCollection<TopicSummary>();
            RefreshTopics = RelayAsyncSimpleCommand.Create(LoadAllTopics, CanLoadAllTopics);
            SelectedProduct = new DisplayableProduct {ProductName = "Opportunity", ProductId = "OpportunityId" };
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
                .Where(t => t.Product.ProductName == SelectedProduct.ProductName)
                .ToList()
                .ForEach(topic => TopicsToList.Add(topic));
            canLoad = true;
        }

        public IAsyncCommand RefreshTopics { get; set; }

        public async void RefreshDocuments()
        {
            await LoadAllTopics();
        }
    }
}
