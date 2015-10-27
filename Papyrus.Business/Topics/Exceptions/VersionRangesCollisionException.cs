using System;
using System.Collections.Generic;

namespace Papyrus.Business.Topics.Exceptions
{
    public class VersionRangesCollisionException : Exception
    {
        private static readonly string BaseMessage = "Following ranges are colliding with any Range:\n";

        public VersionRangesCollisionException(List<EditableVersionRange> conflictedRanges) : base(ConstructMessageFrom(conflictedRanges))
        {
            var rangesToShow = ConstructMessageFrom(conflictedRanges);
        }

        private static string ConstructMessageFrom(List<EditableVersionRange> conflictedRanges)
        {
            var rangesToShow = "";
            foreach (var editableVersionRange in conflictedRanges)
            {
                rangesToShow += editableVersionRange.FromVersion.VersionName + "-" +
                                editableVersionRange.ToVersion.VersionName + "\n";
            }
            return BaseMessage + rangesToShow;
        }
    }
}