namespace Papyrus.Business.Topics
{
    public class Collision
    {
        public EditableVersionRange FirstVersionRange { get; private set; }
        public EditableVersionRange SecondVersionRange { get; private set; }

        public Collision(EditableVersionRange firstVersionRange, EditableVersionRange secondVersionRange)
        {
            FirstVersionRange = firstVersionRange;
            SecondVersionRange = secondVersionRange;
        }
    }
}