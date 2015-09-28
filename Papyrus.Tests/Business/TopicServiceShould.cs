using NSubstitute;
using NUnit.Framework;
using Papyrus.Business;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class TopicServiceShould
    {
        // TODO:
        //   update topic
        //   topic which is not related to a product cannot be saved
        //   topic without ranges cannot be saved 
        //   topic with id cannot be saved 
        //   topic without id cannot be updated


        [Test]
        public void save_a_topic_when_it_is_created()
        {
            var topicRepo = Substitute.For<TopicRepository>();
            var service = new TopicService(topicRepo);
            var topic = new Topic()
                .ForProduct("AnyProductId");
            var anyVersionRange = new VersionRange(fromVersion: "AnyVersionId", toVersion: "AnotherVersionId");
            topic.AddVersionRange(anyVersionRange);

            service.Create(topic);

            topicRepo.Received().Save(topic);
        }

        [Test]
        public void update_a_topic_of_the_library()
        {
            var topicRepo = Substitute.For<TopicRepository>();
            var service = new TopicService(topicRepo);
            var topic = new Topic()
                .WithId("AnyTopicId")
                .ForProduct("AnyProductId");
            var anyVersionRange = new VersionRange(fromVersion: "AnyVersionId", toVersion: "AnotherVersionId");
            topic.AddVersionRange(anyVersionRange);

            service.Update(topic);

            topicRepo.Received().Update(topic);
        }
    }
}