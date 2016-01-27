using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;
using Papyrus.Desktop;
using Papyrus.Desktop.Features.Topics;

namespace Papyrus.Tests.View {
    [TestFixture]
    public class TopicViewModelShould {
        private TopicService topicService;
        private ProductRepository productRepository;
        private NotificationSender notificationSender;

        [SetUp]
        public void SetUp() {
            topicService = Substitute.For<TopicService>(null, null);
            productRepository = Substitute.For<ProductRepository>();
            notificationSender = Substitute.For<NotificationSender>();
        }

        [Test]
        public async Task show_an_error_dialog_to_the_user_when_he_tries_to_save_a_topic_with_invalid_documents() {
            var topic = ATopicContainingADocumentWithEmptyTitle();
            var viewModel = new TopicVM(topicService, productRepository, topic, notificationSender);

            await ExecuteSaveTopicCommand(viewModel);

            notificationSender.Received(1).SendNotification("Debe facilitar un título para todos los documentos de este version range.");
        }
        
        [Test]
        public async Task show_an_error_dialog_to_the_user_when_he_tries_to_save_a_topic_with_invalid_version_range() {
            var topic = ATopicContainingADescendentVersionRange();
            var viewModel = new TopicVM(topicService, productRepository, topic, notificationSender);

            await ExecuteSaveTopicCommand(viewModel);

            notificationSender.Received(1).SendNotification("No pueden existir rangos de versiones descendientes. \n" +
                                                            "Compruebe que todos los rangos para este topic son ascendientes.");
        }

        private static async Task ExecuteSaveTopicCommand(TopicVM viewModel) {
            await viewModel.SaveTopic.ExecuteAsync(new object());
        }

        private EditableTopic ATopicContainingADescendentVersionRange() {
            var versionRange = new EditableVersionRange {
                FromVersion = new ProductVersion("Any", "Any", DateTime.Today),
                ToVersion = new ProductVersion("Another", "Another", DateTime.Today.AddDays(-2))
            };
            var topic = new EditableTopic {
                Product = new DisplayableProduct(),
                TopicId = "AnyId",
                VersionRanges = new ObservableCollection<EditableVersionRange> { versionRange }
            };
            return topic;
        }

        private static EditableTopic ATopicContainingADocumentWithEmptyTitle() {
            var versionRange = new EditableVersionRange {
                FromVersion = new ProductVersion("Any", "Any", DateTime.Today.AddDays(-2)),
                ToVersion = new ProductVersion("Another", "Another", DateTime.Today)
            };
            versionRange.Documents.Add(new EditableDocument {Title = ""});
            var versionRanges = new ObservableCollection<EditableVersionRange> {
                versionRange
            };
            var topic = new EditableTopic {
                Product = new DisplayableProduct(),
                TopicId = "AnyId",
                VersionRanges = versionRanges
            };
            return topic;
        }
    }
}