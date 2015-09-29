using System;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Topics
{
    public class Topic
    {
        public string TopicId { get; set; }
        public string ProductId { get; set; }
        public VersionRanges VersionRanges { get; set; }

        public Topic()
        {
            VersionRanges = new VersionRanges();
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

    public class VersionRanges
    {
        IList<VersionRange> versions = new List<VersionRange>();  

        public void Add(VersionRange versionRange)
        {
            versions.Add(versionRange);
        }

        public bool Any()
        {
            return versions.Any();
        }
    }
}