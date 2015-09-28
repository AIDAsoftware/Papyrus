namespace Papyrus.Business.Topics
{
    public interface TopicRepository
    {
        void Save(Topic topic);
        void Update(Topic topic);
    }
}