using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Annotations;

namespace Papyrus.Desktop.Features.Topics
{
    public class TopicVM : INotifyPropertyChanged
    {
        private readonly TopicRepository topicRepository;
        private readonly string topicId;
        private readonly ProductRepository productRepository;
        private readonly TopicService topicService;
        public ObservableCollection<DisplayableProduct> Products { get; set; }
        public ObservableCollection<string> Languages { get; set; }

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

        private EditableDocument currentDocument;
        public EditableDocument CurrentDocument
        {
            get { return currentDocument; }
            set
            {
                currentDocument = value;
                OnPropertyChanged("CurrentDocument");
            }
        }

        private string selectedLanguage;
        public string SelectedLanguage {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");
            }
        }

        public TopicVM()
        {
            Languages = new ObservableCollection<string>();
            Products = new ObservableCollection<DisplayableProduct>();
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
            CurrentDocument = new EditableDocument();
            await LoadProductsAndLanguages();
            if (topicId != null)
            {
                await LoadTopicToEdit();
            }
        }

        private async Task LoadTopicToEdit()
        {
            EditableTopic = await topicRepository.GetEditableTopicById(topicId);
            EditableTopic.Product = Products.First(p => p.ProductId == EditableTopic.Product.ProductId);
            SelectedLanguage = "es-ES";
            CurrentDocument = EditableTopic.VersionRanges.First().Documents.First();
        }

        private async Task LoadProductsAndLanguages()
        {
            Languages.Add("es-ES");
            Languages.Add("en-GB");
            var allProductsAvailable = await productRepository.GetAllDisplayableProducts();
            allProductsAvailable.ForEach(p => Products.Add(p));
        }

        public async Task SaveTopic()
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}