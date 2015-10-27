using System;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Topics.Exceptions;

namespace Papyrus.Business.Topics
{
    public class TopicService
    {
        private readonly VersionRangeCollisionDetector collisionDetector;
        private TopicRepository TopicRepository { get; set; }

        public TopicService(TopicRepository topicRepo, VersionRangeCollisionDetector collisionDetector)
        {
            this.collisionDetector = collisionDetector;
            TopicRepository = topicRepo;
        }

        public async Task Create(Topic topic)
        {
            await ValidateToSave(topic);
            topic.GenerateRecursiveAutomaticIdIfNeeded();
            await TopicRepository.Save(topic);
        }

        public async Task Update(Topic topic)
        {
            await ValidateToUpdate(topic);
            topic.GenerateRecursiveAutomaticIdIfNeeded();
            await TopicRepository.Update(topic);
        }

        private async Task ValidateToUpdate(Topic topic)
        {
            if (IsNotDefined(topic.TopicId))
                throw new CannotUpdateTopicsWithoutTopicIdDeclaredException();
            if (HasNotAnyVersionRange(topic))
                throw new CannotUpdateTopicsWithNoVersionRangesException();
            if (await collisionDetector.IsThereAnyCollisionFor(topic))
                throw new VersionRangesCollsionException();
        }

        public async Task Delete(Topic topic)
        {
            if (string.IsNullOrWhiteSpace(topic.TopicId))
            {
                throw new CannotDeleteTopicsWithoutTopicIdAssignedException();
            }
            await TopicRepository.Delete(topic);
        }

        private async Task ValidateToSave(Topic topic)
        {
            if (!IsNotDefined(topic.TopicId))
                throw new CannotSaveTopicsWithDefinedTopicIdException();
            if (IsNotDefined(topic.ProductId))
                throw new CannotSaveTopicsWithNoRelatedProductException();
            if (HasNotAnyVersionRange(topic))
                throw new CannotSaveTopicsWithNoVersionRangesException();
            if (await collisionDetector.IsThereAnyCollisionFor(topic))
                throw new VersionRangesCollsionException();
        }

        private static bool HasNotAnyVersionRange(Topic topic)
        {
            return topic.HasNotAnyVersionRange();
        }

        private static bool IsNotDefined(string property)
        {
            return String.IsNullOrEmpty(property);
        }
    }
}