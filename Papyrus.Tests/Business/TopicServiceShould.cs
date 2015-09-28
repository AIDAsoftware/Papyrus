using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Topics;
using Papyrus.Business.Topics.Exceptions;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class TopicServiceShould
    {
        // TODO:
        //   topic which is not related to a product cannot be saved
        //   topic without ranges cannot be saved 
        //   topic with id cannot be saved 
        //   topic with no version ranges cannot be updated

        private TopicRepository topicRepo;
        private TopicService topicService;
        private VersionRange anyVersionRange;

        [SetUp]
        public void SetUp()
        {
            topicRepo = Substitute.For<TopicRepository>();
            topicService = new TopicService(topicRepo);
            anyVersionRange = new VersionRange(fromVersion: "AnyVersionId", toVersion: "AnotherVersionId");
        }

        [Test]
        public void save_a_topic_when_it_is_created()
        {
            var topic = new Topic()
                .ForProduct("AnyProductId");
            topic.AddVersionRange(anyVersionRange);

            topicService.Create(topic);

            topicRepo.Received().Save(topic);
        }

        [Test]
        [ExpectedException(typeof(CannotUpdateWithoutTopicIdDeclaredException))]
        public void throw_an_exception_when_trying_to_update_a_topic_without_id()
        {
            var topic = new Topic()
                .ForProduct("AnyProductId");
            topic.AddVersionRange(anyVersionRange);

            topicService.Update(topic);
        }

        [Test]
        public void update_a_topic_of_the_library()
        {
            var topic = new Topic()
                .WithId("AnyTopicId")
                .ForProduct("AnyProductId");
            topic.AddVersionRange(anyVersionRange);

            topicService.Update(topic);

            topicRepo.Received().Update(topic);
        }
    }
}