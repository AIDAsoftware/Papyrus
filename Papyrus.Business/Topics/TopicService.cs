using System;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Topics.Exceptions;

namespace Papyrus.Business.Topics
{
    public class TopicService
    {
        public TopicRepository TopicRepository { get; set; }

        public TopicService(TopicRepository topicRepo)
        {
            TopicRepository = topicRepo;
        }

        public async Task Create(Topic topic)
        {
            ValidateToSave(topic);
            topic.GenerateRecursiveAutomaticIdIfNeeded();
            await TopicRepository.Save(topic);
        }

        public async Task Update(Topic topic)
        {
            ValidateToUpdate(topic);
            topic.GenerateRecursiveAutomaticIdIfNeeded();
            await TopicRepository.Update(topic);
        }

        private static void ValidateToUpdate(Topic topic)
        {
            if (IsNotDefined(topic.TopicId))
                throw new CannotUpdateTopicsWithoutTopicIdDeclaredException();
            if (HasNotAnyVersionRange(topic))
                throw new CannotUpdateTopicsWithNoVersionRangesException();
        }

        public async Task Delete(Topic topic)
        {
            await TopicRepository.Delete(topic);
        }

        private static void ValidateToSave(Topic topic)
        {
            if (!IsNotDefined(topic.TopicId))
                throw new CannotSaveTopicsWithDefinedTopicIdException();
            if (IsNotDefined(topic.ProductId))
                throw new CannotSaveTopicsWithNoRelatedProductException();
            if (HasNotAnyVersionRange(topic))
                throw new CannotSaveTopicsWithNoVersionRangesException();
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