using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Features.Documents;

namespace Papyrus.Desktop.Features.Topics {
    public class TopicsGridVM
    {
        private readonly TopicRepository topicRepository;

        public ObservableCollection<TopicSummary> TopicsToList { get; }
        public TopicSummary SelectedTopic { get; set; }

        public TopicsGridVM()
        {
            TopicsToList = new ObservableCollection<TopicSummary>();
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
            TopicsToList.Clear();
            (await topicRepository.GetAllTopicsSummaries()).ForEach(topic => TopicsToList.Add(topic));
        }

        public async void RefreshDocuments()
        {
            await LoadAllTopics();
        }
    }

    public class DesignModeDocumentsGridVM : DocumentsGridVM {
        public DesignModeDocumentsGridVM()
        {
        }
    }

}
