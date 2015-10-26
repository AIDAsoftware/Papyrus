namespace Papyrus.Business.Products
{
    public class FullVersionRange
    {
        public string FirstVersionId { get; private set; }
        public string LatestVersionId { get; private set; }

        public FullVersionRange(string firstVersionId, string latestVersionId)
        {
            FirstVersionId = firstVersionId;
            LatestVersionId = latestVersionId;
        }
    }
}