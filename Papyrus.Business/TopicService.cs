namespace Papyrus.Business
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
            TopicRepository.Save(topic);
        }
    }
}