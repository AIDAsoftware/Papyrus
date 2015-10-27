using System;
using System.Collections.Generic;

namespace Papyrus.Business.Topics.Exceptions
{
    public class VersionRangesCollisionException : Exception
    {
        private static readonly string BaseMessage = "Following ranges are colliding with any Range:\n";

        public VersionRangesCollisionException(List<Collision> conflictedRanges) : base(ConstructMessageFrom(conflictedRanges))
        {
        }

        private static string ConstructMessageFrom(List<Collision> conflictedRanges)
        {
            var rangesToShow = "";
            foreach (var editableVersionRange in conflictedRanges)
            {
                rangesToShow += "(" + editableVersionRange.FirstVersionRange.FromVersion.VersionName + ", " +
                                editableVersionRange.FirstVersionRange.ToVersion.VersionName + ") with " +
                                "(" + editableVersionRange.SecondVersionRange.FromVersion.VersionName + ", " +
                                editableVersionRange.SecondVersionRange.ToVersion.VersionName + ")" + "\n";
            }
            return BaseMessage + rangesToShow;
        }
    }
}