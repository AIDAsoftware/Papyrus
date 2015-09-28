using System.Collections;
using System.Collections.Generic;

namespace Papyrus.Business
{
    public class Topic
    {
        public string ProductId { get; set; }
        public List<VersionRange> VersionRanges { get; set; }

        public Topic()
        {
            VersionRanges = new List<VersionRange>();
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
    }
}