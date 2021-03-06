using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.VersionRanges
{
    public class VersionRangeCollisionDetector
    {
        private readonly ProductRepository productRepository;
        private List<ProductVersion> Versions { get; set; }

        public VersionRangeCollisionDetector(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public virtual async Task<List<Collision>> CollisionsFor(Topic topic)
        {
            await LoadAllVersionsFor(topic.ProductId);
            var rangesWithAllVersions = RangesWithAllVersionsFor(topic);
            return CollisionsIn(rangesWithAllVersions);
        }

        private List<RangeWithAllVersions> RangesWithAllVersionsFor(Topic topic)
        {
            var versionRanges = new List<VersionRange>(topic.VersionRanges);
            var rangesWithAllVersions = MapToRangeWithAllVersions(versionRanges);
            return rangesWithAllVersions;
        }

        private async Task LoadAllVersionsFor(string productId)
        {
            Versions = (await productRepository.GetProduct(productId)).Versions;
        }

        private List<Collision> CollisionsIn(List<RangeWithAllVersions> rangesWithAllVersions)
        {
            var allCollisions = new List<Collision>();
            var checkedRanges = new List<RangeWithAllVersions>();
            foreach (var currentRange in rangesWithAllVersions)
            {
                checkedRanges.Add(currentRange);
                var rangesToCheck = rangesWithAllVersions.Except(checkedRanges);
                var collisions = currentRange.CollissionsWith(rangesToCheck);
                allCollisions.AddRange(collisions);
            }
            return allCollisions;
        }

        private List<RangeWithAllVersions> MapToRangeWithAllVersions(IEnumerable<VersionRange> versionRanges)
        {
            return versionRanges.Select(ToRangeWithAllVersions).ToList();
        }

        private RangeWithAllVersions ToRangeWithAllVersions(VersionRange versionRange)
        {
            var versions = AllVersionsContainedIn(versionRange);
            var rangeWithAllVersions = new RangeWithAllVersions(versions, versionRange);
            return rangeWithAllVersions;
        }

        private List<ProductVersion> AllVersionsContainedIn(VersionRange versionRange)
        {
            return Versions.Where(v => versionRange.FromVersion.Release <= v.Release &&
                                       v.Release <= versionRange.ToVersion.Release).ToList();
        }
    }
}