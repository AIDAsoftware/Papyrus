using NSubstitute;
using NUnit.Framework;
using Papyrus.Business;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class TopicServiceShould
    {
        // TODO:
        //   save topic 
        //   get topic by Id
        //   update topic
        //   Get all topics
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

    }
}