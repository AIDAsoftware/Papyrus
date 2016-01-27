using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;
using Papyrus.Tests.Infrastructure.Repositories.TopicRepository;

namespace Papyrus.Tests.Builders {
    public class TopicBuilder {
        private string productId;
        private string topicId;
        private readonly List<VersionRange> versionRanges;

        public TopicBuilder(string productId, string topicId) {
            this.productId = productId;
            this.topicId = topicId;
            versionRanges = new List<VersionRange>();
        }

        public TopicBuilder WithVersionRanges(params VersionRangeBuilder[] versionRanges) {
            this.versionRanges.AddRange(versionRanges.Select(x => x.Build()));
            return this;
        }

        public Topic Build() {
            var topic = new Topic(productId).WithId(topicId);
            foreach (var versionRange in versionRanges) {
                topic.AddVersionRange(versionRange);
            }
            return topic;
        }
    }
}