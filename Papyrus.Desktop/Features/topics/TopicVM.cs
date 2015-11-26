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
        private readonly ProductRepository productRepository;
        public EditableTopic EditableTopic { get; protected set; }

        public IAsyncCommand SaveTopic { get; set; }
        public RelayCommand<Window> DeleteTopic { get; set; }

        public ObservableCollection<ProductVersion> FromVersions { get; private set; }
        public ObservableCollection<ProductVersion> ToVersions { get; private set; }

        public TopicVM()
        {
            SaveTopic = RelayAsyncSimpleCommand.Create(TryToSaveCurrentTopic, CanSaveTopic);
            DeleteTopic = new RelayCommand<Window>(DeleteCurrentTopic);
            FromVersions = new ObservableCollection<ProductVersion>();
            ToVersions = new ObservableCollection<ProductVersion>();
        }

        private TopicVM(TopicService topicService, ProductRepository productRepository) : this()
        {
            this.topicService = topicService;
            this.productRepository = productRepository;
        }

        public TopicVM(TopicService topicService, ProductRepository productRepository, EditableTopic topic) 
            : this(topicService, productRepository)
        {
            EditableTopic = topic;
        }

        private async Task TryToSaveCurrentTopic()
        {
            try
            {
                await SaveCurrentTopic();
            }
            catch (Exception exception)
            {
                EventBus.Send(new OnUserMessageRequest(exception.Message));
            }
        }

        private async Task SaveCurrentTopic()
        {
            var topic = EditableTopic.ToTopic();
            if (string.IsNullOrEmpty(topic.TopicId))
            {
                await topicService.Create(topic);
                EditableTopic.TopicId = topic.TopicId;
            }
            else
            {
                await topicService.Update(topic);
            }

            EventBus.Send(new OnUserMessageRequest("Topic Saved!"));
        }

        //TODO: How to make it not void? It could be a trouble if product can't be deleted in backend
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

        public async void Initialize()
        {
            var versions = await productRepository.GetAllVersionsFor(EditableTopic.Product.ProductId);
            versions.ForEach(v => FromVersions.Add(v));
            versions.ForEach(v => ToVersions.Add(v));
            ToVersions.Add(new LastProductVersion());
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