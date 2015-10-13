using System;

namespace Papyrus.Business.Topics
{
    public class Topic
    {
        public string TopicId { get; set; }
        public string ProductId { get; set; }
        public VersionRanges VersionRanges { get; set; }

        private Topic()
        {
            VersionRanges = new VersionRanges();
        }

        public Topic(string productId) : this()
        {
            ProductId = productId;
        }

        public Topic WithId(string topicid)
        {
            TopicId = topicid;
            return this;
        }

        public Topic ForProduct(string productId)
        {
            ProductId = productId;
            return this;
        }

        public void AddVersionRange(VersionRange versionRange)
        {
            VersionRanges.Add(versionRange);
        }

        public void GenerateAutomaticId()
        {
            TopicId = Guid.NewGuid().ToString();
        }

        public bool HasNotAnyVersionRange()
        {
            return !VersionRanges.HasAnyRange();
        }
    }
}