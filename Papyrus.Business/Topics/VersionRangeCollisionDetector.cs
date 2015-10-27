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
            foreach (var versionRange in versionRanges)
            {
                var isThereCollision =
                    versionRanges.Where(vr => vr != versionRange)
                                .Any(vr => ReleaseFor(versionRange.ToVersionId).Equals(ReleaseFor(vr.FromVersionId)));
                if (isThereCollision)
                {
                    return true;
                }
            }
            return false;
        }

        private DateTime ReleaseFor(string versionId)
        {
            return Versions.First(vr => versionId == vr.VersionId).Release;
        }
    }
}