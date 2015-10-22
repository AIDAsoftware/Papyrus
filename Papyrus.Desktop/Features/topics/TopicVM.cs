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
        private readonly TopicService topicService;
        public EditableTopic EditableTopic { get; }

        public IAsyncCommand SaveTopic { get; set; }

        public TopicVM()
        {
            SaveTopic = RelayAsyncSimpleCommand.Create(SaveCurrentTopic, CanSaveTopic);
        }

        public TopicVM(TopicService topicService) : this()
        {
            this.topicService = topicService;
        }

        public TopicVM(TopicService topicService, EditableTopic topic) : this(topicService)
        {
            EditableTopic = topic;
        }

        private async Task SaveCurrentTopic()
        {
            var topic = EditableTopic.ToTopic();
            if (string.IsNullOrEmpty(topic.TopicId))
            {
                await topicService.Create(topic);
            }
            else
            {
                await topicService.Update(topic);
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