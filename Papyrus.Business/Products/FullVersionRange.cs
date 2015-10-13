namespace Papyrus.Business.Products
{
    public class FullVersionRange
    {
        public string FirstVersionId { get; }
        public string LatestVersionId { get; }

        public FullVersionRange(string firstVersionId, string latestVersionId)
        {
            FirstVersionId = firstVersionId;
            LatestVersionId = latestVersionId;
        }
    }
}