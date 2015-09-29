using System;
using System.Collections.Generic;

namespace Papyrus.Business.Topics
{
    public class Topic
    {
        public string ProductId { get; set; }
        public List<VersionRange> VersionRanges { get; set; }
        public string TopicId { get; set; }

        public Topic()
        {
            VersionRanges = new List<VersionRange>();
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
    }
}