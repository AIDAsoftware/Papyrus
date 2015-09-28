namespace Papyrus.Business
{
    public interface TopicRepository
    {
        void Save(Topic topic);
        void Update(Topic topic);
    }
}