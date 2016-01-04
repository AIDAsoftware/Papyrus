using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Papyrus.Business.Topics.Exceptions;

namespace Papyrus.Business.Topics
{
    public class TopicService
    {
        private readonly VersionRangeCollisionDetector collisionDetector;
        private TopicCommandRepository TopicRepository { get; set; }

        public TopicService(TopicCommandRepository topicRepo, VersionRangeCollisionDetector collisionDetector)
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
            await ValidateVersionRangesCollisionsFor(topic);
        }

        private async Task ValidateVersionRangesCollisionsFor(Topic topic)
        {
            var conflictedRanges = (await collisionDetector.CollisionsFor(topic));
            if (conflictedRanges.Any())
            {
                throw new VersionRangesCollisionException(conflictedRanges);
            }
        }

        public async Task Delete(string topicId)
        {
            if (string.IsNullOrWhiteSpace(topicId))
            {
                throw new CannotDeleteTopicsWithoutTopicIdAssignedException();
            }
            await TopicRepository.Delete(topicId);
        }

        private async Task ValidateToSave(Topic topic)
        {
            if (!IsNotDefined(topic.TopicId))
                throw new CannotSaveTopicsWithDefinedTopicIdException();
            if (IsNotDefined(topic.ProductId))
                throw new CannotSaveTopicsWithNoRelatedProductException();
            if (HasNotAnyVersionRange(topic))
                throw new CannotSaveTopicsWithNoVersionRangesException();
            await ValidateVersionRangesCollisionsFor(topic);
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