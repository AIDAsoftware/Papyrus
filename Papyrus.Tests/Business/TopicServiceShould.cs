using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.Topics.Exceptions;
using Papyrus.Business.VersionRanges;
using Papyrus.Business.VersionRanges.Exceptions;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class TopicServiceShould
    {
        private TopicCommandRepository topicRepo;
        private VersionRangeCollisionDetector collisionDetector;
        private TopicService topicService;
        private VersionRange anyVersionRange;
        private string anyProductId;
        private readonly ProductVersion anyVersion = new ProductVersion("Any", "Any", DateTime.MaxValue);

        [SetUp]
        public void SetUp()
        {
            topicRepo = Substitute.For<TopicCommandRepository>();
            var productRepository = Substitute.For<ProductRepository>();
            collisionDetector = Substitute.For<VersionRangeCollisionDetector>(productRepository);
            topicService = new TopicService(topicRepo, collisionDetector);
            anyVersionRange = new VersionRange(fromVersion: anyVersion, toVersion: anyVersion);
            anyProductId = "AnyProductId";
        }

        [Test]
        public void fail_when_trying_to_save_topics_with_no_related_product()
        {
            var topic = new Topic(null);
            topic.AddVersionRange(anyVersionRange);

            Func<Task> create = async () => await topicService.Create(topic);

            create.ShouldThrow<CannotSaveTopicsWithNoRelatedProductException>();
        }

        [Test]
        public void fail_when_trying_to_save_topics_with_no_ranges()
        {
            var topic = new Topic(anyProductId);

            Func<Task> create = async () => await topicService.Create(topic);

            create.ShouldThrow<CannotSaveTopicsWithNoVersionRangesException>();
        }

        [Test]
        public void fail_when_trying_to_save_topics_with_id()
        {
            var topic = new Topic(anyProductId)
                            .WithId("AnyTopicId");
            topic.AddVersionRange(anyVersionRange);

            Func<Task> create = async () => await topicService.Create(topic);

            create.ShouldThrow<CannotSaveTopicsWithDefinedTopicIdException>();
        }

        [Test]
        public void fail_when_try_to_create_a_topic_with_version_ranges_that_collide()
        {
            var topic = new Topic(anyProductId);
            topic.AddVersionRange(anyVersionRange);
            topic.AddVersionRange(anyVersionRange);
            var anyEditableVersionRange = new VersionRange(
                fromVersion: new ProductVersion("Any", "2.0", DateTime.Today),
                toVersion: new ProductVersion("Any", "3.0", DateTime.Today)
            );
            var anyListToRepresentConflictedVersionRanges = new List<Collision> {new Collision(
                anyEditableVersionRange, anyEditableVersionRange
            )};
            collisionDetector.CollisionsFor(topic).Returns(Task.FromResult(anyListToRepresentConflictedVersionRanges));

            Func<Task> createTopic = async () => await topicService.Create(topic);
            
            createTopic.ShouldThrow<VersionRangesCollisionException>()
                .WithMessage("(2.0 -- 3.0) collide with (2.0 -- 3.0)\n");
        }


        [Test]
        public async Task save_a_topic_when_it_is_created()
        {
            var topic = new Topic(anyProductId);
            topic.AddVersionRange(anyVersionRange);
            collisionDetector.CollisionsFor(topic).Returns(new List<Collision>());

            await topicService.Create(topic);

            topicRepo.Received().Save(topic);
            topicRepo.Received().Save(Arg.Is<Topic>(t => !String.IsNullOrEmpty(t.TopicId)));
        }

        [Test]
        public async Task fail_when_trying_to_update_a_topic_without_id()
        {
            var topic = new Topic(anyProductId);
            topic.AddVersionRange(anyVersionRange);

            Func<Task> update = async () => await topicService.Update(topic);

            update.ShouldThrow<CannotUpdateTopicsWithoutTopicIdDeclaredException>();
        }

        [Test]
        public async Task fail_when_trying_to_update_a_topic_with_no_ranges()
        {
            var topic = new Topic(anyProductId)
                .WithId("AnyTopicId");

            Func<Task> update = async () => await topicService.Update(topic);

            update.ShouldThrow<CannotUpdateTopicsWithNoVersionRangesException>();
        }

        [Test]
        public async Task fail_when_try_to_update_a_topic_with_version_ranges_that_collide()
        {
            var topic = new Topic(anyProductId)
                .WithId("AnyTopicId");
            topic.AddVersionRange(anyVersionRange);
            topic.AddVersionRange(anyVersionRange);
            var anyEditableVersionRange = new VersionRange(
                fromVersion: new ProductVersion("Any", "2.0", DateTime.Today),
                toVersion: new ProductVersion("Any", "3.0", DateTime.Today)
            );
            var anyListToRepresentConflictedVersionRanges = new List<Collision> {new Collision(
                anyEditableVersionRange, anyEditableVersionRange
            )};
            collisionDetector.CollisionsFor(topic).Returns(Task.FromResult(anyListToRepresentConflictedVersionRanges));

            Func<Task> createTopic = async () => await topicService.Update(topic);

            createTopic.ShouldThrow<VersionRangesCollisionException>()
                .WithMessage("(2.0 -- 3.0) collide with (2.0 -- 3.0)\n");
        }

        [Test]
        public async Task update_a_topic_of_the_library()
        {
            var topic = new Topic(anyProductId)
                .WithId("AnyTopicId");
            topic.AddVersionRange(anyVersionRange);
            collisionDetector.CollisionsFor(topic).Returns(Task.FromResult(new List<Collision>()));

            await topicService.Update(topic);

            topicRepo.Received().Update(topic);
        }

        [Test]
        public async Task delete_a_topic_from_the_library()
        {
            var topic = new Topic(anyProductId).WithId("AnyTopicId");

            await topicService.Delete(topic.TopicId);

            topicRepo.Received().Delete(topic.TopicId);
        }

        [Test]
        public async Task fail_when_try_to_delete_a_topic_without_topic_id_assigned()
        {
            var topic = new Topic(anyProductId);

            Func<Task> delete = async () => await topicService.Delete(topic.TopicId);

            delete.ShouldThrow<CannotDeleteTopicsWithoutTopicIdAssignedException>();
        }
    }
}