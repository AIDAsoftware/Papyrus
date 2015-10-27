using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.Topics.Exceptions;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class TopicServiceShould
    {
        private TopicRepository topicRepo;
        private VersionRangeCollisionDetector collisionDetector;
        private TopicService topicService;
        private VersionRange anyVersionRange;
        private string anyProductId;

        [SetUp]
        public void SetUp()
        {
            topicRepo = Substitute.For<TopicRepository>();
            var productRepository = Substitute.For<ProductRepository>();
            collisionDetector = Substitute.For<VersionRangeCollisionDetector>(productRepository);
            topicService = new TopicService(topicRepo, collisionDetector);
            anyVersionRange = new VersionRange(fromVersionId: null, toVersionId: null);
            anyProductId = "AnyProductId";
        }

        [Test]
        [ExpectedException(typeof(CannotSaveTopicsWithNoRelatedProductException))]
        public async void fail_when_trying_to_save_topics_with_no_related_product()
        {
            var topic = new Topic(null);
            topic.AddVersionRange(anyVersionRange);

            await topicService.Create(topic);
        }

        [Test]
        [ExpectedException(typeof(CannotSaveTopicsWithNoVersionRangesException))]
        public async void fail_when_trying_to_save_topics_with_no_ranges()
        {
            var topic = new Topic(anyProductId);

            await topicService.Create(topic);
        }

        [Test]
        [ExpectedException(typeof(CannotSaveTopicsWithDefinedTopicIdException))]
        public async void fail_when_trying_to_save_topics_with_id()
        {
            var topic = new Topic(anyProductId)
                            .WithId("AnyTopicId");
            topic.AddVersionRange(anyVersionRange);

            await topicService.Create(topic);
        }

        [Test, ExpectedException(typeof(VersionRangesCollisionException))]
        public async void fail_when_try_to_create_a_topic_with_version_ranges_that_collide()
        {
            var topic = new Topic(anyProductId);
            topic.AddVersionRange(anyVersionRange);
            topic.AddVersionRange(anyVersionRange);
            var anyListToRepresentConflictedVersionRanges = new List<EditableVersionRange> {new EditableVersionRange()};
            collisionDetector.VersionRangesWithCollisionsFor(topic).Returns(Task.FromResult(anyListToRepresentConflictedVersionRanges));

            await topicService.Create(topic);
        }


        [Test]
        public async void save_a_topic_when_it_is_created()
        {
            var topic = new Topic(anyProductId);
            topic.AddVersionRange(anyVersionRange);
            collisionDetector.VersionRangesWithCollisionsFor(topic).Returns(Task.FromResult(new List<EditableVersionRange>()));

            await topicService.Create(topic);

            topicRepo.Received().Save(topic);
            topicRepo.Received().Save(Arg.Is<Topic>(t => !String.IsNullOrEmpty(t.TopicId)));
        }

        [Test]
        [ExpectedException(typeof(CannotUpdateTopicsWithoutTopicIdDeclaredException))]
        public async Task fail_when_trying_to_update_a_topic_without_id()
        {
            var topic = new Topic(anyProductId);
            topic.AddVersionRange(anyVersionRange);

            await topicService.Update(topic);
        }

        [Test]
        [ExpectedException(typeof(CannotUpdateTopicsWithNoVersionRangesException))]
        public async Task fail_when_trying_to_update_a_topic_with_no_ranges()
        {
            var topic = new Topic(anyProductId)
                .WithId("AnyTopicId");

            await topicService.Update(topic);
        }

        [Test, ExpectedException(typeof(VersionRangesCollisionException))]
        public async Task fail_when_try_to_update_a_topic_with_version_ranges_that_collide()
        {
            var topic = new Topic(anyProductId)
                .WithId("AnyTopicId");
            topic.AddVersionRange(anyVersionRange);
            topic.AddVersionRange(anyVersionRange);
            var anyListToRepresentConflictedVersionRanges = new List<EditableVersionRange> {new EditableVersionRange()};
            collisionDetector.VersionRangesWithCollisionsFor(topic).Returns(Task.FromResult(anyListToRepresentConflictedVersionRanges));

            await topicService.Update(topic);
        }

        [Test]
        public async Task update_a_topic_of_the_library()
        {
            var topic = new Topic(anyProductId)
                .WithId("AnyTopicId");
            topic.AddVersionRange(anyVersionRange);
            collisionDetector.VersionRangesWithCollisionsFor(topic).Returns(Task.FromResult(new List<EditableVersionRange>()));

            await topicService.Update(topic);

            topicRepo.Received().Update(topic);
        }

        [Test]
        public async Task delete_a_topic_from_the_library()
        {
            var topic = new Topic(anyProductId).WithId("AnyTopicId");

            await topicService.Delete(topic);

            topicRepo.Received().Delete(topic);
        }

        [Test]
        [ExpectedException(typeof(CannotDeleteTopicsWithoutTopicIdAssignedException))]
        public async Task fail_when_try_to_delete_a_topic_without_topic_id_assigned()
        {
            var topic = new Topic(anyProductId);
            await topicService.Delete(topic);
        }
    }
}