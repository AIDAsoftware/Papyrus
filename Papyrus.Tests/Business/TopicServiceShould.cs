using NSubstitute;
using NUnit.Framework;
using Papyrus.Business;
using Papyrus.Business.Topics;
using Papyrus.Business.Topics.Exceptions;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class TopicServiceShould
    {
        private TopicRepository topicRepo;
        private TopicService topicService;
        private VersionRange anyVersionRange;
        private string anyProductId;

        [SetUp]
        public void SetUp()
        {
            topicRepo = Substitute.For<TopicRepository>();
            topicService = new TopicService(topicRepo);
            anyVersionRange = new VersionRange(fromVersion: "AnyVersionId", toVersion: "AnotherVersionId");
            anyProductId = "AnyProductId";
        }

        [Test]
        [ExpectedException(typeof(CannotSaveTopicsWithNoRelatedProductException))]
        public void fail_when_trying_to_save_topics_with_no_related_product()
        {
            var topic = new Topic();
            topic.AddVersionRange(anyVersionRange);

            topicService.Create(topic);
        }

        [Test]
        [ExpectedException(typeof(CannotSaveTopicsWithNoVersionRangesException))]
        public void fail_when_trying_to_save_topics_with_no_ranges()
        {
            var topic = new Topic().ForProduct(anyProductId);

            topicService.Create(topic);
        }

        [Test]
        [ExpectedException(typeof(CannotSaveTopicsWithDefinedTopicIdException))]
        public void fail_when_trying_to_save_topics_with_id()
        {                                      
            var topic = new Topic()
                        .WithId("AnyTopicId")
                        .ForProduct(anyProductId);
            topic.AddVersionRange(anyVersionRange);

            topicService.Create(topic);
        }

        [Test]
        public void save_a_topic_when_it_is_created()
        {
            var topic = new Topic()
                .ForProduct(anyProductId);
            topic.AddVersionRange(anyVersionRange);

            topicService.Create(topic);

            topicRepo.Received().Save(topic);
        }

        [Test]
        [ExpectedException(typeof(CannotUpdateWithoutTopicIdDeclaredException))]
        public void fail_when_trying_to_update_a_topic_without_id()
        {
            var topic = new Topic()
                .ForProduct(anyProductId);
            topic.AddVersionRange(anyVersionRange);

            topicService.Update(topic);
        }

        [Test]
        [ExpectedException(typeof(CannotUpdateTopicsWithNoVersionRangesException))]
        public void fail_when_trying_to_update_a_topic_with_no_ranges()
        {
            var topic = new Topic()
                .WithId("AnyTopicId")
                .ForProduct(anyProductId);

            topicService.Update(topic);
        }

        [Test]
        public void update_a_topic_of_the_library()
        {
            var topic = new Topic()
                .WithId("AnyTopicId")
                .ForProduct(anyProductId);
            topic.AddVersionRange(anyVersionRange);

            topicService.Update(topic);

            topicRepo.Received().Update(topic);
        }
    }
}