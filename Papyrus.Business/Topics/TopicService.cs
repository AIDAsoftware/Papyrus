using System;
using System.Linq;
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

        public void Create(Topic topic)
        {
            ValidateToSave(topic);
            TopicRepository.Save(topic);
        }

        public void Update(Topic topic)
        {
            if (String.IsNullOrEmpty(topic.TopicId))
                throw new CannotUpdateWithoutTopicIdDeclaredException();
            TopicRepository.Update(topic);
        }

        private static void ValidateToSave(Topic topic)
        {
            if (!IsNotDefined(topic.TopicId))
                throw new CannotSaveTopicsWithDefinedTopicIdException();
            if (IsNotDefined(topic.ProductId))
                throw new CannotSaveTopicsWithNoRelatedProductException();
            if (!topic.VersionRanges.Any())
                throw new CannotSaveTopicsWithNoVersionRangesException();
        }

        private static bool IsNotDefined(string property)
        {
            return String.IsNullOrEmpty(property);
        }
    }
}