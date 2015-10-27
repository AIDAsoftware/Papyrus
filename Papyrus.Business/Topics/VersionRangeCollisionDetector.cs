using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Topics
{
    public class VersionRangeCollisionDetector
    {
        private readonly ProductRepository productRepository;
        private List<ProductVersion> Versions { get; set; }


        public VersionRangeCollisionDetector(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<bool> IsThereAnyCollisionFor(Topic topic)
        {
            Versions = await productRepository.GetAllVersionsFor(topic.ProductId);
            var versionRanges = topic.VersionRanges;
            return versionRanges.Any(versionRange => 
                DoesVersionRangeCollideWithAnyRangeIn(versionRange, versionRanges));
        }

        private bool DoesVersionRangeCollideWithAnyRangeIn(VersionRange versionRange, VersionRanges versionRanges)
        {
            var fromVersionId = versionRange.FromVersionId;
            return versionRanges.Where(vr => vr != versionRange)
                    .Any(vr => IsVersionIncludedInVersionRange(fromVersionId, vr));
        }

        private bool IsVersionIncludedInVersionRange(string fromVersionId, VersionRange versionRange)
        {
            return ReleaseFor(versionRange.FromVersionId) <= ReleaseFor(fromVersionId) &&
                   ReleaseFor(fromVersionId) <= ReleaseFor(versionRange.ToVersionId);
        }

        private DateTime ReleaseFor(string versionId)
        {
            return Versions.First(vr => versionId == vr.VersionId).Release;
        }
    }
}