using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Annotations;
using Papyrus.Desktop.Util.Command;

namespace Papyrus.Desktop.Features.Topics
{
    public class TopicVM : INotifyPropertyChanged
    {
        private readonly TopicRepository topicRepository;
        private readonly string topicId;
        private readonly ProductRepository productRepository;
        private readonly TopicService topicService;
        private EditableTopic editableTopic;

        public EditableTopic EditableTopic
        {
            get { return editableTopic; }
            set
            {
                editableTopic = value;
                OnPropertyChanged();
            }
        }

        public IAsyncCommand SaveTopic { get; set; }

        public TopicVM()
        {
            SaveTopic = RelayAsyncSimpleCommand.Create(SaveCurrentTopic, CanSaveTopic);
        }

        public TopicVM(ProductRepository productRepository, TopicService topicService) : this()
        {
            this.productRepository = productRepository;
            this.topicService = topicService;
        }

        public TopicVM(ProductRepository productRepository, TopicService topicService, TopicRepository topicRepository, string topicId) : this(productRepository, topicService)
        {
            this.topicRepository = topicRepository;
            this.topicId = topicId;
        }

        public async Task Initialize()
        {
            EditableTopic = new EditableTopic
            {
                Product = new DisplayableProduct()
            };
            if (topicId != null)
            {
                EditableTopic = await topicRepository.GetEditableTopicById(topicId);
            }
        }

        private async Task SaveCurrentTopic()
        {
            var topic = EditableTopic.ToTopic();
            if (string.IsNullOrEmpty(topicId))
            {
                await topicService.Create(topic);
            }
            else
            {
                await topicService.Update(topic.WithId(topicId));
            }
        }

        private bool CanSaveTopic()
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}