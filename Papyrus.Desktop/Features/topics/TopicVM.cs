using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Annotations;
using Papyrus.Desktop.Util.Command;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop.Features.Topics
{
    public class TopicVM : INotifyPropertyChanged
    {
        private readonly TopicService topicService;
        public EditableTopic EditableTopic { get; protected set; }

        public IAsyncCommand SaveTopic { get; set; }
        public RelayCommand<Window> DeleteTopic { get; set; }

        public TopicVM()
        {
            SaveTopic = RelayAsyncSimpleCommand.Create(SaveCurrentTopic, CanSaveTopic);
            DeleteTopic = new RelayCommand<Window>(DeleteCurrentTopic);
        }

        private TopicVM(TopicService topicService) : this()
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

            EventBus.Raise(new OnUserMessageRequest("Topic Saved!"));
        }

        private async void DeleteCurrentTopic(Window window)
        {
            var topic = EditableTopic.ToTopic();
            await topicService.Delete(topic);
            window.Close();
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

        public void Handle(OnUserMessageRequest domainEvent)
        {
            EventBus.Raise(domainEvent);
        }
    }

    public class DesignModeTopicVM : TopicVM
    {
        public DesignModeTopicVM()
        {
            EditableTopic = new EditableTopic
            {
                VersionRanges = new ObservableCollection<EditableVersionRange>
                {
                    new EditableVersionRange
                    {
                        FromVersion = new ProductVersion("AnyId", "1.0", DateTime.Today),
                        ToVersion = new ProductVersion("AnyId", "2.0", DateTime.Today)
                    },
                    new EditableVersionRange
                    {
                        FromVersion = new ProductVersion("AnyId", "3.0", DateTime.Today),
                        ToVersion = new ProductVersion("AnyId", "4.0", DateTime.Today)
                    }
                }
            };
        }
    }
}