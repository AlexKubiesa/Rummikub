using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RummikubLib;

namespace RummikubTests
{
    [TestFixture]
    public class CombinatoricsTests
    {
        public static IEnumerable<TestCaseData> GetSubsetsTestCases = new[]
        {
            new TestCaseData(
                    new int[0],
                    new[] { new int[0] })
                .SetName("Empty set"),

            new TestCaseData(
                    new[] { 1 },
                    new[] { new int[0], new[] { 1 } })
                .SetName("Set of size 1"),

            new TestCaseData(
                    new[] { 1, 2 },
                    new[] { new int[0], new[] { 1 }, new[] { 2 }, new[] { 1, 2 } })
                .SetName("Set of size 2"),

            new TestCaseData(
                    new[] { 1, 2, 3 },
                    new[] { new int[0], new[] { 1 }, new[] { 2 }, new[] { 3 },
                        new[] { 1, 2 }, new[] { 2, 3 }, new[] { 1, 3 }, new[] { 1, 2, 3 } })
                .SetName("Set of size 3")
        };

        [Test]
        [TestCaseSource(nameof(GetSubsetsTestCases))]
        public void TestGetSubsets(IEnumerable<int> set, IEnumerable<IEnumerable<int>> expectedSubsets)
        {
            var actualSubsets = set.GetSubsets();
            Assert.That(actualSubsets, Is.EquivalentTo(expectedSubsets).Using(new CollectionEqualityComparer<int>()));
        }
    }
}