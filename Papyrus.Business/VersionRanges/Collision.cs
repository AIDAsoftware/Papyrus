namespace Papyrus.Business.VersionRanges
{
    public class Collision
    {
        public VersionRange FirstVersionRange { get; private set; }
        public VersionRange SecondVersionRange { get; private set; }

        public Collision(VersionRange firstVersionRange, VersionRange secondVersionRange) {
            FirstVersionRange = firstVersionRange;
            SecondVersionRange = secondVersionRange;
        }
    }
}