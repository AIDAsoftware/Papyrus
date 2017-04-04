using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.Topics.Exceptions;
using Papyrus.Business.VersionRanges;
using Papyrus.Desktop.Annotations;
using Papyrus.Desktop.Util.Command;
using Papyrus.Infrastructure.Core;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop.Features.Topics
{
    public class TopicVM : INotifyPropertyChanged
    {
        private readonly TopicService topicService;
        private readonly ProductRepository productRepository;
        private readonly NotificationSender notificationSender;
        public EditableTopic EditableTopic { get; protected set; }
        public EditableTopic LastTopicSaved { get; private set; }

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

        public TopicVM(TopicService topicService, ProductRepository productRepository, EditableTopic topic, NotificationSender notificationSender) : this()
        {
            this.topicService = topicService;
            this.productRepository = productRepository;
            this.notificationSender = notificationSender;
            EditableTopic = topic;
            LastTopicSaved = topic.Clone();
        }

        private async Task TryToSaveCurrentTopic()
        {
            try {
                await SaveCurrentTopic();
            }
            catch (Exception exception)
            {
                notificationSender.SendNotification(exception.Message);
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

            LastTopicSaved = EditableTopic.Clone();
            EventBus.Send(new OnTopicSaved());
        }

        //TODO: How to make it not void? It could be a trouble if product can't be deleted in backend
        private async void DeleteCurrentTopic(Window window)
        {
            try {
                await topicService.Delete(EditableTopic.TopicId);
                EventBus.Send(new OnTopicRemoved());
            }
            catch (CannotDeleteTopicsWithoutTopicIdAssignedException) {
                notificationSender.SendNotification("No se puede borrar un topic no guardado");
            }
            
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
            var product = await productRepository.GetProduct(EditableTopic.Product.ProductId);
            var versions = product.Versions;
            FromVersions.AddRange(versions);
            FillToVersions(versions);
        }

        private void FillToVersions(List<ProductVersion> versions) {
            ToVersions.AddRange(versions);
            ToVersions.Add(new LastProductVersion());
        }

        public bool TopicIsSaved() {
            return EditableTopic.Equals(LastTopicSaved);
        }


        public string Order
        {
            get { return EditableTopic.Order; }
            set { EditableTopic.Order = !value.All(char.IsDigit) ? "200" : value; }
        }

    }

    internal class OnTopicSaved {
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