using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Topics
{
    public class VersionRangeCollisionDetector
    {
        private readonly ProductRepository productRepository;

        public VersionRangeCollisionDetector(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<bool> IsThereAnyCollisionFor(Topic topic)
        {
            foreach (var versionRange in topic.VersionRanges)
            {
                var isThereCollision = topic.VersionRanges.Any(range => versionRange.ToVersionId == range.FromVersionId);
                if (isThereCollision)
                {
                    return true;
                }
            }
            return false;
        }
    }
}