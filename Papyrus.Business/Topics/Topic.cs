using System;
using Papyrus.Business.VersionRanges;

namespace Papyrus.Business.Topics
{
    public class Topic
    {
        public string TopicId { get; set; }
        public string ProductId { get; set; }
        public VersionRanges.VersionRanges VersionRanges { get; set; }

        public Topic(string productId)
        {
            VersionRanges = new VersionRanges.VersionRanges();
            ProductId = productId;
        }

        public Topic WithId(string topicid)
        {
            TopicId = topicid;
            return this;
        }

        public void AddVersionRange(VersionRange versionRange)
        {
            VersionRanges.Add(versionRange);
        }

        public void GenerateRecursiveAutomaticIdIfNeeded()
        {
            if (string.IsNullOrWhiteSpace(TopicId))
            {
                TopicId = Guid.NewGuid().ToString();
            }
            foreach (var versionRange in VersionRanges)
            {
                versionRange.GenerateRecursiveAutomaticId();
            }
        }

        public bool HasNotAnyVersionRange()
        {
            return !VersionRanges.HasAnyRange();
        }
    }
}