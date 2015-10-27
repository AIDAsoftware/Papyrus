using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class VersionRangeCollisionDetectorShould
    {
        // TODO
        //   - when to version of a version range is equal to fromVersion in any other range
        //   - when from version of a version range is contained  by other range
        // Examples:
        //   (1-2), (2-3)
        //   (1-3), (2-4)
    }
}